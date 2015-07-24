using System;
using System.IO;
using System.Threading.Tasks;
using Shots.Web.Models;

namespace Shots.Web.Services.Interface
{
    public interface IShotsService
    {
        /// <summary>
        ///     Gets the identifier for the vendor.
        ///     Used by Shots to identify devices.
        /// </summary>
        /// <value>
        ///     The identifier of the vendor.
        /// </value>
        string IdentifierForVendor { get; }

        bool IsAuthenticated { get; }

        /// <summary>
        ///     Gets the current user.
        /// </summary>
        /// <value>
        ///     The current user.
        /// </value>
        AuthenticatedUserInfo CurrentUser { get; }

        /// <summary>
        ///     Checks the email against the api.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>Success if is available.</returns>
        Task<BaseResponse> CheckEmailAsync(string email);

        /// <summary>
        ///     Gets the home list.
        /// </summary>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="perPage">Results per page</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        Task<HomeListResponse> GetHomeListAsync(string lastId = null, int perPage = 8, bool configureLoadMore = true);

        void AttachLoadMore(HomeListResponse response);

        /// <summary>
        ///     Gets the discover list.
        /// </summary>
        /// <returns></returns>
        Task<BaseListResponse> GetDiscoverListAsync();

        /// <summary>
        ///     Gets the shot item.
        /// </summary>
        /// <param name="id">The resource id.</param>
        /// <returns></returns>
        Task<SingleItemResponse> GetShotItemAsync(string id);

        /// <summary>
        ///     Likes the shot item.
        /// </summary>
        /// <param name="id">The resource id.</param>
        /// <param name="on">if set to <c>true</c> [on].</param>
        /// <returns></returns>
        Task<LikeResponse> LikeShotItemAsync(string id, bool on = true);

        /// <summary>
        ///     Gets the suggested users for the current account.
        /// </summary>
        /// <returns></returns>
        Task<SuggestedResponse> GetSuggestedUsersAsync();

        /// <summary>
        ///     Gets the user info by id.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns></returns>
        Task<UserInfoReponse> GetUserAsync(string id);

        /// <summary>
        ///     Gets the user info by name.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <returns></returns>
        Task<UserInfoReponse> GetUserByNameAsync(string name);

        /// <summary>
        ///     Gets the user's followers.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        Task<FollowersResponse> GetUserFollowersAsync(string id, string lastId = null, bool configureLoadMore = true);

        void AttachLoadMore(FollowersResponse response);

        /// <summary>
        ///     Gets the user's following.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        Task<FollowingResponse> GetUserFollowingAsync(string id, string lastId = null, bool configureLoadMore = true);

        /// <summary>
        ///     Gets the user's following for the specified timeframe.
        /// </summary>
        /// <param name="since">Since when to request following.</param>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        Task<FollowingResponse> GetUserFollowingAsync(DateTime since, string id, string lastId = null,
            bool configureLoadMore = true);

        void AttachLoadMore(FollowingResponse response);

        /// <summary>
        ///     Gets the user's list.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <param name="configureLoadMore">If set to [true] it will configure HasMoreItem and LoadMoreItemsFunc.</param>
        /// <returns></returns>
        Task<UserListWithSuggestionResponse> GetUserListAsync(string id, string lastId = null,
            bool configureLoadMore = true);

        void AttachLoadMore(UserListWithSuggestionResponse response);

        /// <summary>
        ///     Logins to shots.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<BaseResponse> LoginAsync(string username, string password);

        /// <summary>
        ///     Logouts this instance.
        /// </summary>
        void Logout();

        /// <summary>
        ///     Registers a new account on shots.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <param name="name">The first name.</param>
        /// <param name="birthday">The birthday.</param>
        /// <param name="imageData">The image data. Use null to not include any.</param>
        /// <returns></returns>
        Task<BaseResponse> RegisterAsync(string username, string password, string email, string name,
            DateTime birthday, Stream imageData);

        /// <summary>
        ///     Sends the SMS verification code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        Task<SmsVerificationResponse> SendSmsVerificationCode(string countryCode, string phoneNumber);

        /// <summary>
        ///     Verifies the SMS code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Task<BaseResponse> VerifySmsCode(string code);

        /// <summary>
        ///     Toggles the shottie's relationship.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="add">if set to <c>true</c> It adds the user as a friend, else removes him.</param>
        /// <returns></returns>
        Task<BaseResponse> ToggleFriend(string id, bool add = true);

        /// <summary>
        ///     Search for users.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Task<UserListResponse> SearchUsersAsync(string query);
    }
}