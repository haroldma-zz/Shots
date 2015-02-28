using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shots.Api.Models;
using Shots.Api.Utilities;

namespace Shots.Api
{
    /// <summary>
    ///     Provides read and write operations to the Shots API.
    /// </summary>
    public class ShotsService : IShotsService
    {
        private readonly AppSettingsHelper _appSettingsHelper;
        private readonly CredentialHelper _credentialHelper;
        private string _consumer;
        private string _identifierForVendor;
        private string _secret;

        public ShotsService(AppSettingsHelper appSettingsHelper, CredentialHelper credentialHelper)
        {
            _appSettingsHelper = appSettingsHelper;
            _credentialHelper = credentialHelper;

            var credentials = _credentialHelper.GetCredentials(ShotsConstants.CredentialResouceName);

            if (credentials == null) return;

            _consumer = credentials.UserName;
            credentials.RetrievePassword();
            _secret = credentials.Password;

            CurrentUser = _appSettingsHelper.ReadJsonAs<AuthenticatedUserInfo>(ShotsConstants.CredentialResouceName);
        }

        /// <summary>
        ///     Gets the identifier for the vendor.
        ///     Used by Shots to identify devices.
        /// </summary>
        /// <value>
        ///     The identifier of the vendor.
        /// </value>
        public string IdentifierForVendor
        {
            get
            {
                return _identifierForVendor ??
                       (_identifierForVendor = _appSettingsHelper.Read("IdentifierForVendor", Guid.NewGuid().ToString()));
            }
        }

        public bool IsAuthenticated { get { return CurrentUser != null; } }

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
            const string path = ShotsConstants.UserLoginPath;
            var data = GetDefaultData(path);
            data.Add("email", email);

            return await PostAsync<BaseResponse>(path, data);
        }

        /// <summary>
        ///     Gets the home list.
        /// </summary>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        public async Task<HomeListResponse> GetHomeListAsync(string lastId = null)
        {
            const string path = ShotsConstants.ListHomePath;
            var data = GetDefaultData(path);
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            return await PostAsync<HomeListResponse>(path, data);
        }

        /// <summary>
        ///     Gets the discover list.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseListResponse> GetDiscoverListAsync()
        {
            const string path = ShotsConstants.ListDiscoverPath;
            var data = GetDefaultData(path);

            return await PostAsync<BaseListResponse>(path, data);
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
            var resp = await PostAsync<BasePageListResponse>(path, data);

            // To make everything nicer, we convert it to a SingleItemResponse manually
            return new SingleItemResponse
            {
                Item = resp.Items != null ? resp.Items.FirstOrDefault() : null,
                Message = resp.Message,
                Status = resp.Status,
                ServerTime = resp.ServerTime
            };
        }

        /// <summary>
        /// Likes the shot item.
        /// </summary>
        /// <param name="id">The resource id.</param>
        /// <param name="on">if set to <c>true</c> [on].</param>
        /// <returns></returns>
        public async Task<LikeResponse> LikeShotItemAsync(string id, bool on = true)
        {
            var path = on ? ShotsConstants.LikeOnPath : ShotsConstants.LikeOffPath;
            var data = GetDefaultData(path);
            // Haven't seen any other type yet
            data.Add("type", "photo");
            data.Add("resource_id", id);

            return await PostAsync<LikeResponse>(path, data);
        }

        /// <summary>
        ///     Gets the suggested users for the current account.
        /// </summary>
        /// <returns></returns>
        public async Task<SuggestedResponse> GetSuggestedUsersAsync()
        {
            const string path = ShotsConstants.SuggestedPath;
            var data = GetDefaultData(path);
            return await PostAsync<SuggestedResponse>(path, data);
        }

        /// <summary>
        ///     Gets the specified id user info.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns></returns>
        public async Task<UserInfoReponse> GetUserAsync(string id)
        {
            const string path = ShotsConstants.UserLoadPath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id);
            return await PostAsync<UserInfoReponse>(path, data);
        }

        /// <summary>
        ///     Gets the user's followers.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        public async Task<FollowersResponse> GetUserFollowersAsync(string id, string lastId = null)
        {
            const string path = ShotsConstants.UserFollowersPath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id == "me" ? CurrentUser.Id : id);
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            return await PostAsync<FollowersResponse>(path, data);
        }

        /// <summary>
        ///     Gets the user's following.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        public Task<FollowingResponse> GetUserFollowingAsync(string id, string lastId = null)
        {
            return GetUserFollowingAsync(DateTime.MinValue, id, lastId);
        }

        /// <summary>
        ///     Gets the user's following for the specified timeframe.
        /// </summary>
        /// <param name="since">Since when to request following.</param>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        public async Task<FollowingResponse> GetUserFollowingAsync(DateTime since, string id, string lastId = null)
        {
            const string path = ShotsConstants.UserFollowingPath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id == "me" ? CurrentUser.Id : id);
            if (since != DateTime.MinValue) data.Add("since", since.ToUnixTimestamp().ToString());
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            return await PostAsync<FollowingResponse>(path, data);
        }

        /// <summary>
        ///     Gets the user's list.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        public async Task<UserListResponse> GetUserListAsync(string id, string lastId = null)
        {
            const string path = ShotsConstants.ListHomePath;
            var data = GetDefaultData(path);
            data.Add("request_user_id", id == "me" ? CurrentUser.Id : id);
            if (!string.IsNullOrEmpty(lastId)) data.Add("last_id", lastId);

            return await PostAsync<UserListResponse>(path, data);
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

            var resp = await PostAsync<LoginResponse>(path, data);

            if (resp.Status == Status.Success &&
                resp.Keys != null) SaveAuthentication(resp.UserInfo, resp.Keys.Consumer, resp.Keys.Secret);

            return resp;
        }

        /// <summary>
        ///     Registers a new account on shots.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="birthday">The birthday.</param>
        /// <param name="imageData">The image data. Use null to not include any.</param>
        /// <returns></returns>
        public async Task<BaseResponse> RegisterAsync(string username, string password, string email, string firstName,
                                                      string lastName, DateTime birthday, Stream imageData)
        {
            const string path = ShotsConstants.UserNewPath;
            var data = GetDefaultData(path);
            data.Add("identifierForVendor", IdentifierForVendor);
            data.Add("username", username);
            data.Add("password", password);
            data.Add("email", email);
            data.Add("fname", firstName);
            data.Add("lname", lastName);
            data.Add("birthday", birthday.ToUnixTimestamp().ToString());

            LoginResponse resp;

            if (imageData != null) resp = await PostAsync<LoginResponse>(path, data, imageData);
            else resp = await PostAsync<LoginResponse>(ShotsConstants.UserNewPath, data);

            if (resp.Status == Status.Success &&
                resp.Keys != null) SaveAuthentication(resp.UserInfo, resp.Keys.Consumer, resp.Keys.Secret);

            return resp;
        }

        #region Helpers

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
            _credentialHelper.SaveCredentials(ShotsConstants.CredentialResouceName, consumer, secret);
            _appSettingsHelper.WriteAsJson(ShotsConstants.CredentialResouceName, authenticatedUserInfo);

            _consumer = consumer;
            _secret = secret;
            CurrentUser = authenticatedUserInfo;
        }

        /// <summary>
        ///     Resets the authentication information.
        /// </summary>
        private void ResetAuthentication()
        {
            _credentialHelper.DeleteCredentials(ShotsConstants.CredentialResouceName);
            _appSettingsHelper.WriteAsJson(ShotsConstants.CredentialResouceName, null);

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
                {"appId", "8"},
                {"appVersion", "3.1.7"},
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

                    return parseResp;
                }
            }
        }

        #endregion
    }
}