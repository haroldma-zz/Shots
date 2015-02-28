using System;
using System.IO;
using System.Threading.Tasks;
using Shots.Api.Models;

namespace Shots.Api
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
        /// <returns></returns>
        Task<HomeListResponse> GetHomeListAsync(string lastId = null);

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
        /// Likes the shot item.
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
        ///     Gets the specified id user info.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns></returns>
        Task<UserInfoReponse> GetUserAsync(string id);

        /// <summary>
        ///     Gets the user's followers.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        Task<FollowersResponse> GetUserFollowersAsync(string id, string lastId = null);

        /// <summary>
        ///     Gets the user's following.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        Task<FollowingResponse> GetUserFollowingAsync(string id, string lastId = null);

        /// <summary>
        ///     Gets the user's following for the specified timeframe.
        /// </summary>
        /// <param name="since">Since when to request following.</param>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        Task<FollowingResponse> GetUserFollowingAsync(DateTime since, string id, string lastId = null);

        /// <summary>
        ///     Gets the user's list.
        /// </summary>
        /// <param name="id">The user id. (Use "me" for the current account)</param>
        /// <param name="lastId">The id of the last item. (Paging)</param>
        /// <returns></returns>
        Task<UserListResponse> GetUserListAsync(string id, string lastId = null);

        /// <summary>
        ///     Logins to shots.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<BaseResponse> LoginAsync(string username, string password);

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
        Task<BaseResponse> RegisterAsync(string username, string password, string email, string firstName,
            string lastName, DateTime birthday, Stream imageData);
    }
}