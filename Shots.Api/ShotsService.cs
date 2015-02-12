using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shots.Api.Models;
using Shots.Api.Utilities;

namespace Shots.Api
{
    public class ShotsService
    {
        private readonly AppSettingsHelper _appSettingsHelper;
        private string _identifierForVendor;

        public ShotsService(AppSettingsHelper appSettingsHelper)
        {
            _appSettingsHelper = appSettingsHelper;
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

            //TODO Save login state

            return await PostAsync<LoginResponse>(path, data);
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

            if (imageData != null)
                return await PostAsync<LoginResponse>(path, data, imageData);

            //TODO Save login state

            return await PostAsync<LoginResponse>(path, data);
        }

        #region Helpers

        private static HttpClient CreateClient()
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            client.DefaultRequestHeaders.Add("User-Agent", ShotsConstants.ClientHeader);
            return client;
        }

        private SortedDictionary<string, string> GetDefaultData(string path)
        {
            var code = ShotsAuthHelper.GetTimeCode();
            var signature = ShotsAuthHelper.GetSignature(path, code);

            return new SortedDictionary<string, string>
            {
                {"appId", "8"},
                {"appVersion", "3.1.7"},
                {"lang", "en"},
                {"langs", "en"},
                {"locale", "en_US"},
                {"rl_auth", signature},
                {"rl_code", code}
            };
        }

        private async Task<T> PostAsync<T>(string path, IDictionary<string, string> data) where T : BaseResponse, new()
        {
            using (var content = new FormUrlEncodedContent(data))
                return await PostAsync<T>(path, content);
        }

        private async Task<T> PostAsync<T>(string path, IDictionary<string, string> data, Stream imageData)
            where T : BaseResponse, new()
        {
            using (var content = new MultipartFormDataContent("--Boundary+67DE93E465AACAB1"))
            {
                // add dictionary data
                foreach (var pair in data)
                    content.Add(new StringContent(pair.Value), pair.Key);

                // then the image stream
                content.Add(CreateFileContent(imageData, "picture", "picture.jpg", "image/jpeg"));

                return await PostAsync<T>(path, content);
            }
        }

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

        private async Task<T> PostAsync<T>(string path, HttpContent content) where T : BaseResponse, new()
        {
            var url = "https://" + ShotsConstants.ApiBase + path;

            using (var client = CreateClient())
            {
                var resp = await client.PostAsync(url, content);
                var json = await resp.Content.ReadAsStringAsync();

                T parseResp;
                try
                {
                    parseResp = JsonConvert.DeserializeObject<T>(json, new JsonEpochDateTimeConverter(),
                        new JsonIntBoolConverter());
                }
                catch
                {
                    parseResp = new T {Message = "Problem connecting to the cloud."};
                }

                return parseResp;
            }
        }

        #endregion
    }
}