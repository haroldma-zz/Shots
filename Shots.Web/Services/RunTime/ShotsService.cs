using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Newtonsoft.Json;
using Shots.Core.Common;
using Shots.Core.Extensions;
using Shots.Core.Utilities.Interfaces;
using Shots.Web.Converters;
using Shots.Web.Helpers;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.Web.Services.RunTime
{
    /// <summary>
    ///     Provides read and write operations to the Shots API.
    /// </summary>
    public class ShotsService : IShotsService
    {
        private readonly ICredentialUtility _credentialUtility;
        private readonly ISettingsUtility _settingsUtility;
        private string _consumer;
        private string _identifierForVendor;
        private string _secret;

        public ShotsService(ISettingsUtility settingsUtility, ICredentialUtility credentialUtility)
        {
            _settingsUtility = settingsUtility;
            _credentialUtility = credentialUtility;

            var credentials = _credentialUtility.GetCredentials(ShotsConstants.CredentialResouceName);

            if (credentials == null) return;

            _consumer = credentials.Username;
            _secret = credentials.Password;

            CurrentUser = _settingsUtility.ReadJsonAs<AuthenticatedUserInfo>(ShotsConstants.CredentialResouceName);
        }

        /// <summary>
        ///     Gets the identifier for the vendor.
        ///     Used by Shots to identify devices.
        /// </summary>
        /// <value>
        ///     The identifier of the vendor.
        /// </value>
        public string IdentifierForVendor => _identifierForVendor ??
                                             (_identifierForVendor =
                                                 _settingsUtility.Read("IdentifierForVendor", Guid.NewGuid().ToString()))
            ;

        public bool IsAuthenticated => CurrentUser != null;

        /// <summary>
        ///     Gets the current user.
        /// </summary>
        /// <value>
        ///     The current user.
        /// </value>
        public AuthenticatedUserInfo CurrentUser { get; private set; }

        /// <summary>
        ///     Checks the email against the api.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>Success if is available.</returns>
        public async Task<BaseResponse> CheckEmailAsync(string email)
        {
            const string path = ShotsConstants.UserCheckPath;
            var data = GetDefaultData(path);
            data.Add("email", email);

            return await PostAsync<BaseResponse>(path, data);
        }

        /// <summary>
        ///     Gets the home list.
        /// </summary>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="perPage">Results per page</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        public async Task<HomeListResponse> GetHomeListAsync(string lastId = null, int perPage = 8,
            bool configureLoadMore = true)
        {
            const string path = ShotsConstants.ListHomePath;
            var data = GetDefaultData(path);
            data.Add("per_page", perPage.ToString());
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            var response = await PostAsync<HomeListResponse>(path, data).DontMarshall();

            ConfigureAds(response.Items);

            if (configureLoadMore)
                AttachLoadMore(response);

            if (lastId == null && response.Status == Status.Success)
            {
                var item = response.Items?.FirstOrDefault();
                if (item?.Resource?.Description?.Contains("shots.com/update") ?? false)
                    item.Resource.Description =
                        "*Shotty for Shots (Windows) update schedule differs, check the store for updates.*";
            }

            return response;
        }
        
        public void AttachLoadMore(HomeListResponse response)
        {
            if (response.Items == null || response.PageInfo == null || response.Items.All(p => p.Resource == null))
                return;

            response.Items.HasMoreItems = response.PageInfo.EntryCount != 0;
            response.Items.LoadMoreItemsFunc = u =>
            {
                Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                {
                    // Make sure to set configureLoadMore to [false]
                    var resp =
                        await GetHomeListAsync(response.Items.LastOrDefault(p => p.Resource != null)?.Resource.Id ?? "",
                                configureLoadMore: false);

                    if (resp != null)
                    {
                        response.LoggedInUser = resp.LoggedInUser;
                        response.PageInfo = resp.PageInfo;
                        CopyBaseResponse(resp, response);

                        if (resp.Items != null)
                        {
                            response.Items.HasMoreItems = response.PageInfo.EntryCount != 0;
                            response.Items.AddRange(resp.Items);
                        }
                    }
                    return new LoadMoreItemsResult {Count = (uint) (resp?.Items?.Count ?? 0)};
                };
                var loadMorePostsTask = taskFunc();
                return loadMorePostsTask.AsAsyncOperation();
            };
        }

        /// <summary>
        ///     Gets the discover list.
        /// </summary>
        /// <returns></returns>
        public Task<BaseListResponse> GetDiscoverListAsync()
        {
            const string path = ShotsConstants.ListDiscoverPath;
            var data = GetDefaultData(path);

            return PostAsync<BaseListResponse>(path, data);
        }

        /// <summary>
        ///     Gets the shot item.
        /// </summary>
        /// <param name="id">The resource id.</param>
        /// <returns></returns>
        public async Task<SingleItemResponse> GetShotItemAsync(string id)
        {
            const string path = ShotsConstants.PostByResourceIdPath;
            var data = GetDefaultData(path);
            // Haven't seen any other type yet
            data.Add("type", "photo");
            data.Add("resource_id", id);

            // so the api returns a list response
            var resp = await PostAsync<BaseListResponse>(path, data).DontMarshall();

            // To make everything nicer, we convert it to a SingleItemResponse manually
            return new SingleItemResponse
            {
                Item = resp.Items?.FirstOrDefault(),
                Message = resp.Message,
                Status = resp.Status,
                ServerTime = resp.ServerTime
            };
        }

        /// <summary>
        ///     Likes the shot item.
        /// </summary>
        /// <param name="id">The resource id.</param>
        /// <param name="on">if set to <c>true</c> [on].</param>
        /// <returns></returns>
        public Task<LikeResponse> LikeShotItemAsync(string id, bool on = true)
        {
            var path = on ? ShotsConstants.LikeOnPath : ShotsConstants.LikeOffPath;
            var data = GetDefaultData(path);
            // Haven't seen any other type yet
            data.Add("type", "photo");
            data.Add("resource_id", id);

            return PostAsync<LikeResponse>(path, data);
        }

        /// <summary>
        ///     Gets the suggested users for the current account.
        /// </summary>
        /// <returns></returns>
        public Task<SuggestedResponse> GetSuggestedUsersAsync()
        {
            const string path = ShotsConstants.SuggestedPath;
            var data = GetDefaultData(path);
            return PostAsync<SuggestedResponse>(path, data);
        }

        /// <summary>
        ///     Gets the specified id user info.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns></returns>
        public Task<UserInfoReponse> GetUserAsync(string id)
        {
            const string path = ShotsConstants.UserLoadPath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id);
            return PostAsync<UserInfoReponse>(path, data);
        }

        /// <summary>
        ///     Gets the user info by name.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <returns></returns>
        public Task<UserInfoReponse> GetUserByNameAsync(string name)
        {
            const string path = ShotsConstants.UserLoadPath;
            var data = GetDefaultData(path);
            data.Add("request_username", name);
            return PostAsync<UserInfoReponse>(path, data);
        }

        /// <summary>
        ///     Gets the user's followers.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        public async Task<FollowersResponse> GetUserFollowersAsync(string id, string lastId = null,
            bool configureLoadMore = true)
        {
            const string path = ShotsConstants.UserFollowersPath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id == "me" ? CurrentUser.Id : id);
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            var response = await PostAsync<FollowersResponse>(path, data).DontMarshall();
            response.UserId = id;
            // The Items prop implements ISupportIncrementalLoading. Let's use some C# magic to configure it!
            if (configureLoadMore)
                AttachLoadMore(response);

            return response;
        }

        public void AttachLoadMore(FollowersResponse response)
        {
            if (response.Followers == null || response.PageInfo == null) return;

            response.Followers.HasMoreItems = response.PageInfo.EntryCount != 0;
            response.Followers.LoadMoreItemsFunc = u =>
            {
                Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                {
                    // Make sure to set configureLoadMore to [false]
                    var resp =
                        await
                            GetUserFollowersAsync(response.UserId, response.Followers.LastOrDefault()?.Id ?? "", false);

                    if (resp != null)
                    {
                        response.PageInfo = resp.PageInfo;
                        CopyBaseResponse(resp, response);

                        if (resp.Followers != null)
                        {
                            response.Followers.HasMoreItems = response.PageInfo.EntryCount != 0;
                            response.Followers.AddRange(resp.Followers);
                        }
                    }
                    return new LoadMoreItemsResult {Count = (uint) (resp?.Followers?.Count ?? 0)};
                };
                var loadMorePostsTask = taskFunc();
                return loadMorePostsTask.AsAsyncOperation();
            };
        }

        /// <summary>
        ///     Gets the user's following.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        public Task<FollowingResponse> GetUserFollowingAsync(string id, string lastId = null,
            bool configureLoadMore = true)
        {
            return GetUserFollowingAsync(DateTime.MinValue, id, lastId, configureLoadMore);
        }

        /// <summary>
        ///     Gets the user's following for the specified timeframe.
        /// </summary>
        /// <param name="since">Since when to request following.</param>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        public async Task<FollowingResponse> GetUserFollowingAsync(DateTime since, string id, string lastId = null,
            bool configureLoadMore = true)
        {
            const string path = ShotsConstants.UserFollowingPath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id == "me" ? CurrentUser.Id : id);
            if (since != DateTime.MinValue) data.Add("since", since.ToUnixTimestamp().ToString());
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            var response = await PostAsync<FollowingResponse>(path, data).DontMarshall();
            response.Since = since;
            response.UserId = id;

            // The Items prop implements ISupportIncrementalLoading. Let's use some C# magic to configure it!
            if (configureLoadMore)
                AttachLoadMore(response);

            return response;
        }

        public void AttachLoadMore(FollowingResponse response)
        {
            if (response.Following == null || response.PageInfo == null) return;

            response.Following.HasMoreItems = response.PageInfo.EntryCount != 0;
            response.Following.LoadMoreItemsFunc = u =>
            {
                Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                {
                    // Make sure to set configureLoadMore to [false]
                    var resp =
                        await
                            GetUserFollowingAsync(response.Since, response.UserId,
                                response.Following.LastOrDefault()?.Id ?? "", false);

                    if (resp != null)
                    {
                        response.PageInfo = resp.PageInfo;
                        CopyBaseResponse(resp, response);

                        if (resp.Following != null)
                        {
                            response.Following.HasMoreItems = response.PageInfo.EntryCount != 0;
                            response.Following.AddRange(resp.Following);
                        }
                    }
                    return new LoadMoreItemsResult {Count = (uint) (resp?.Following?.Count ?? 0)};
                };
                var loadMorePostsTask = taskFunc();
                return loadMorePostsTask.AsAsyncOperation();
            };
        }

        /// <summary>
        ///     Gets the user's list.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        public async Task<UserListWithSuggestionResponse> GetUserListAsync(string id, string lastId = null,
            bool configureLoadMore = true)
        {
            const string path = ShotsConstants.ListUserPath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id == "me" ? CurrentUser.Id : id);
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            var response = await PostAsync<UserListWithSuggestionResponse>(path, data).DontMarshall();

            ConfigureAds(response.Items);

            // The Items prop implements ISupportIncrementalLoading. Let's use some C# magic to configure it!
            if (configureLoadMore)
                AttachLoadMore(response);

            return response;
        }

        public void AttachLoadMore(UserListWithSuggestionResponse response)
        {
            if (response.Items == null || response.PageInfo == null || response.Items.All(p => p.Resource == null)) return;

                response.Items.HasMoreItems = response.PageInfo.EntryCount != 0;
            response.Items.LoadMoreItemsFunc = u =>
            {
                Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                {
                    // Make sure to set configureLoadMore to [false]
                    var resp =
                        await
                            GetUserListAsync(response.User.Id, response.Items.LastOrDefault(p => p.Ad == null)?.Resource.Id ?? "", false);

                    if (resp != null)
                    {
                        response.PageInfo = resp.PageInfo;
                        response.User = resp.User;
                        CopyBaseResponse(resp, response);

                        if (resp.Items != null)
                        {
                            response.Items.HasMoreItems = response.PageInfo.EntryCount != 0;
                            response.Items.AddRange(resp.Items);
                        }
                    }
                    return new LoadMoreItemsResult {Count = (uint) (resp?.Items?.Count ?? 0)};
                };
                var loadMorePostsTask = taskFunc();
                return loadMorePostsTask.AsAsyncOperation();
            };
        }

        /// <summary>
        ///     Logins to shots.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<BaseResponse> LoginAsync(string username, string password)
        {
            const string path = ShotsConstants.UserLoginPath;
            var data = GetDefaultData(path);
            data.Add("username", username);
            data.Add("password", password);

            var resp = await PostAsync<LoginResponse>(path, data).DontMarshall();

            if (resp.Status == Status.Success &&
                resp.Keys != null) SaveAuthentication(resp.UserInfo, resp.Keys.Consumer, resp.Keys.Secret);

            return resp;
        }

        public void Logout()
        {
            ResetAuthentication();
        }

        /// <summary>
        ///     Registers a new account on shots.
        /// </summary>
        /// <param name="signUpToken">The sign up token used for verification.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <param name="name">The first name.</param>
        /// <param name="birthday">The birthday.</param>
        /// <param name="imageData">The image data. Use null to not include any.</param>
        /// <returns></returns>
        public async Task<BaseResponse> RegisterAsync(string signUpToken, string username, string password, string email,
            string name,
            DateTime birthday, Stream imageData = null)
        {
            const string path = ShotsConstants.SignUpNewPath;
            var data = GetDefaultData(path);
            data.Add("identifierForVendor", IdentifierForVendor);
            data.Add("sign_up_token", signUpToken);
            data.Add("username", username);
            data.Add("password", password);
            data.Add("email", email);
            data.Add("name", name);
            data.Add("birthday", birthday.ToUnixTimestamp().ToString());

            LoginResponse resp;

            if (imageData != null) resp = await PostAsync<LoginResponse>(path, data, imageData).DontMarshall();
            else resp = await PostAsync<LoginResponse>(ShotsConstants.SignUpNewPath, data).DontMarshall();

            if (resp.Status == Status.Success &&
                resp.Keys != null) SaveAuthentication(resp.UserInfo, resp.Keys.Consumer, resp.Keys.Secret);

            return resp;
        }

        /// <summary>
        ///     Sends the SMS verification code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        public Task<SmsVerificationResponse> SendSmsVerificationCode(string countryCode, string phoneNumber)
        {
            const string path = ShotsConstants.SignUpVerifySmsPath;
            var data = GetDefaultData(path);
            data.Add("country_code", countryCode);
            data.Add("phone_number", phoneNumber);
            data.Add("phone_verification_type", "sms");

            return PostAsync<SmsVerificationResponse>(path, data);
        }

        /// <summary>
        ///     Verifies the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="token">The original request's token.</param>
        /// <returns></returns>
        public Task<BaseResponse> VerifyCode(string code, string token)
        {
            const string path = ShotsConstants.SignUpVerifyCodePath;
            var data = GetDefaultData(path);
            data.Add("code", code);
            data.Add("sign_up_token", token);

            return PostAsync<BaseResponse>(path, data);
        }

        /// <summary>
        ///     Toggles the shottie's relationship.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="add">if set to <c>true</c> It adds the user as a friend, else removes him.</param>
        /// <returns></returns>
        public Task<BaseResponse> ToggleFriend(string id, bool add = true)
        {
            var path = add ? ShotsConstants.FriendsAddPath : ShotsConstants.FriendsRemovePath;
            var data = GetDefaultData(path);
            data.Add("friend_id", id);

            return PostAsync<BaseResponse>(path, data);
        }

        /// <summary>
        ///     Search for users.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public Task<UserListResponse> SearchUsersAsync(string query)
        {
            const string path = ShotsConstants.UserSearchPath;
            var data = GetDefaultData(path);
            data.Add("search", query);

            return PostAsync<UserListResponse>(path, data);
        }

        #region Helpers

        /// <summary>
        /// Configures the ads.
        /// </summary>
        /// <param name="items">The items.</param>
        private void ConfigureAds(IncrementalObservableCollection<ShotItem> items)
        {
            if (items != null)
            {
                if (items.Count(p => p.Ad != null) > 1)
                {
                    // remove ads by shots
                    foreach (var shotItem in items.Where(p => p.Ad != null).ToList())
                    {
                        items.Remove(shotItem);
                    }
                }
                items.Add(new ShotItem { Ad = new Ad() });
            }
        }

        /// <summary>
        ///     Copies the base response.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        private void CopyBaseResponse(BaseResponse from, BaseResponse to)
        {
            to.Logout = from.Logout;
            to.Message = from.Message;
            to.ServerTime = from.ServerTime;
            to.Status = from.Status;
            to.Timings = from.Timings;
        }

        /// <summary>
        ///     Creates the HTTP client.
        ///     Contains required headers.
        /// </summary>
        /// <returns></returns>
        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            client.DefaultRequestHeaders.Add("User-Agent", ShotsConstants.ClientHeader);
            return client;
        }

        /// <summary>
        ///     Saves the authentication information.
        /// </summary>
        /// <param name="authenticatedUserInfo">The user information.</param>
        /// <param name="consumer">The consumer key.</param>
        /// <param name="secret">The secret key.</param>
        private void SaveAuthentication(AuthenticatedUserInfo authenticatedUserInfo, string consumer, string secret)
        {
            _credentialUtility.SaveCredentials(ShotsConstants.CredentialResouceName, consumer, secret);
            _settingsUtility.WriteAsJson(ShotsConstants.CredentialResouceName, authenticatedUserInfo);

            _consumer = consumer;
            _secret = secret;
            CurrentUser = authenticatedUserInfo;
        }

        /// <summary>
        ///     Resets the authentication information.
        /// </summary>
        private void ResetAuthentication()
        {
            _credentialUtility.DeleteCredentials(ShotsConstants.CredentialResouceName);
            _settingsUtility.WriteAsJson(ShotsConstants.CredentialResouceName, null);

            _consumer = null;
            _secret = null;
            CurrentUser = null;
        }

        /// <summary>
        ///     Gets the default data dictionary.
        ///     Including auth related parameters.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private SortedDictionary<string, string> GetDefaultData(string path)
        {
            var code = ShotsAuthHelper.GetTimeCode();
            var signature = ShotsAuthHelper.GetSignature(path, code);

            var data = new SortedDictionary<string, string>
            {
                {"appId", "7"},
                {"appVersion", "4.1"},
                {"lang", "en"},
                {"langs", "en"},
                {"locale", "en_US"},
                {"rl_auth", signature},
                {"rl_code", code}
            };

            if (!string.IsNullOrEmpty(_consumer)) data.Add("rl_consumer", _consumer);
            if (!string.IsNullOrEmpty(_secret)) data.Add("rl_secret", _secret);
            if (CurrentUser != null) data.Add("user_id", CurrentUser.Id);

            return data;
        }

        /// <summary>
        ///     Posts the http content.
        /// </summary>
        /// <typeparam name="T">The type of response, must inherit BaseResponse</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="data">The dictionary data.</param>
        /// <returns></returns>
        private async Task<T> PostAsync<T>(string path, IDictionary<string, string> data) where T : BaseResponse, new()
        {
            using (var content = new FormUrlEncodedContent(data)) return await PostAsync<T>(path, content);
        }

        /// <summary>
        ///     Posts the http content.
        /// </summary>
        /// <typeparam name="T">The type of response, must inherit BaseResponse</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="data">The dictionary data.</param>
        /// <param name="imageData">The image data.</param>
        /// <returns></returns>
        private async Task<T> PostAsync<T>(string path, IDictionary<string, string> data, Stream imageData)
            where T : BaseResponse, new()
        {
            using (var content = new MultipartFormDataContent("--Boundary+67DE93E465AACAB1"))
            {
                // add dictionary data
                foreach (var pair in data) content.Add(new StringContent(pair.Value), pair.Key);

                // then the image stream
                content.Add(CreateFileContent(imageData, "picture", "picture.jpg", "image/jpeg"));

                return await PostAsync<T>(path, content);
            }
        }

        /// <summary>
        ///     Creates a StreamContent for file upload.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="name">The name.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        private StreamContent CreateFileContent(Stream stream, string name, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"" + name + "\"",
                FileName = "\"" + fileName + "\""
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }

        /// <summary>
        ///     Posts the http content.
        /// </summary>
        /// <typeparam name="T">The type of response, must inherit BaseResponse</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="content">The content to post.</param>
        /// <returns></returns>
        private async Task<T> PostAsync<T>(string path, HttpContent content) where T : BaseResponse, new()
        {
            var url = "https://" + ShotsConstants.ApiBase + path;

            using (var client = CreateHttpClient())
            {
                using (var resp = await client.PostAsync(url, content))
                {
                    var json = await resp.Content.ReadAsStringAsync();

                    T parseResp;

                    try
                    {
                        parseResp = JsonConvert.DeserializeObject<T>(json, new JsonEpochDateTimeConverter(),
                            new JsonIntBoolConverter());
                    }
                    catch
                    {
                        parseResp = new T {Message = "Problem connecting to Shots."};
                    }

                    if (parseResp.Status != Status.Success || resp.IsSuccessStatusCode) return parseResp;

                    parseResp.Status = Status.Failed;
                    if (string.IsNullOrEmpty(parseResp.Message))
                        parseResp.Message = "Problem connecting to Shots.";

                    return parseResp;
                }
            }
        }

        #endregion
    }
}