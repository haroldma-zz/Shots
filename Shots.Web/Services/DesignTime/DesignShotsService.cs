using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shots.Core.Common;
using Shots.Web.Models;
using Shots.Web.Services.Interface;

namespace Shots.Web.Services.DesignTime
{
    public class DesignShotsService : IShotsService
    {
        public string IdentifierForVendor
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated => true;

        public AuthenticatedUserInfo CurrentUser => new AuthenticatedUserInfo
        {
            FirstName = "Liane V",
            Username = "lovelianev",
            LastName = "",
            Bio = "Shots on me��",
            Color = 8,
            Dob = new DateTime(1970, 1, 1),
            Gender = "f",
            ProfilePhotoBig =
                "http://rocklivewests3-a.akamaihd.net/17665497/profile/9m96fmyj3pyb0w6lq4hr-200.jpg",
            ProfilePhotoSmall =
                "http://rocklivewests3-a.akamaihd.net/17665497/profile/9m96fmyj3pyb0w6lq4hr-200.jpg"
        };

        public Task<BaseResponse> CheckEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<HomeListResponse> GetHomeListAsync(string lastId = null, int perPage = 8, bool configureLoadMore = true)
        {
            return Task.FromResult(new HomeListResponse
            {
                Status = Status.Success,
                Items = new IncrementalObservableCollection<ShotItem>(CreateShotItems())
            });
        }

        public void AttachLoadMore(HomeListResponse response)
        {
            throw new NotImplementedException();
        }

        public Task<BaseListResponse> GetDiscoverListAsync()
        {
            return Task.FromResult(new BaseListResponse
            {
                Status = Status.Success,
                Items = CreateShotItems()
            });
        }

        public Task<SingleItemResponse> GetShotItemAsync(string id)
        {
            return Task.FromResult(new SingleItemResponse {Item = CreateShotItems()[0]});
        }

        public Task<LikeResponse> LikeShotItemAsync(string id, bool @on = true)
        {
            throw new NotImplementedException();
        }

        public Task<SuggestedResponse> GetSuggestedUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserInfoReponse> GetUserAsync(string id)
        {
            return Task.FromResult(new UserInfoReponse {UserInfo = CurrentUser});
        }

        public Task<UserInfoReponse> GetUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<FollowersResponse> GetUserFollowersAsync(string id, string lastId = null, bool configureLoadMore = true)
        {
            return Task.FromResult(new FollowersResponse
            {
                Status = Status.Success,
                Followers = new IncrementalObservableCollection<UserInfo>(CreateUserList())
            });
        }

        public void AttachLoadMore(FollowersResponse response)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResponse> GetUserFollowingAsync(string id, string lastId = null, bool configureLoadMore = true)
        {
            return GetUserFollowingAsync(DateTime.UtcNow, id, lastId);
        }

        public Task<FollowingResponse> GetUserFollowingAsync(DateTime since, string id, string lastId = null, bool configureLoadMore = true)
        {
            return Task.FromResult(new FollowingResponse
            {
                Status = Status.Success,
                Following = new IncrementalObservableCollection<UserInfo>(CreateUserList())
            });
        }

        public void AttachLoadMore(FollowingResponse response)
        {
            throw new NotImplementedException();
        }

        public Task<UserListWithSuggestionResponse> GetUserListAsync(string id, string lastId = null, bool configureLoadMore = true)
        {
            return Task.FromResult(new UserListWithSuggestionResponse
            {
                Status = Status.Success,
                Suggestions = CreateUserList().Select(p => p as SimpleUserInfo).ToList(),
                Items = new IncrementalObservableCollection<ShotItem>(CreateShotItems())
            });
        }

        public void AttachLoadMore(UserListWithSuggestionResponse response)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> RegisterAsync(string username, string password, string email, string firstName,
            string lastName, DateTime birthday,
            Stream imageData)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> ToggleFriend(string id, bool add = true)
        {
            throw new NotImplementedException();
        }

        public Task<UserListResponse> SearchUsersAsync(string query)
        {
            return Task.FromResult(new UserListResponse
            {
                Users = CreateUserList(),
                Status = Status.Success
            });
        }

        private List<UserInfo> CreateUserList()
        {
            return new List<UserInfo>
            {
                new UserInfo
                {
                    FirstName = "Liane V",
                    LastName = "",
                    Username = "lianev",
                    Bio = "Shots on me��",
                    Color = 8,
                    ProfilePhotoBig =
                        "http://rocklivewests3-a.akamaihd.net/17665497/profile/9m96fmyj3pyb0w6lq4hr.jpg",
                    ProfilePhotoSmall =
                        "http://rocklivewests3-a.akamaihd.net/17665497/profile/9m96fmyj3pyb0w6lq4hr-200.jpg"
                },
                new UserInfo
                {
                    FirstName = "Hailey",
                    LastName = "Baldwin",
                    Username = "janedoe",
                    Color = 8,
                    ProfilePhotoBig =
                        "http://rocklivewests3-a.akamaihd.net/3213286/profile/qcv3q0uieeb8ewz9onfj.jpg",
                    ProfilePhotoSmall =
                        "http://rocklivewests3-a.akamaihd.net/3213286/profile/qcv3q0uieeb8ewz9onfj-200.jpg"
                }
            };
        }

        private List<ShotItem> CreateShotItems()
        {
            return new List<ShotItem>
            {
                new ShotItem
                {
                    Resource = new Resource
                    {
                        Description = "@nashgrier ✌️��",
                        LikeCount = 1234,
                        Likes = new List<Like>
                        {
                            new Like
                            {
                                Fname = "Jane",
                                Lname = "Doe",
                                Username = "janedoe"
                            }
                        },
                        FsVenueInfo = new FsVenueInfo
                        {
                            FsVenueName = "Los Angeles, CA",
                            Id = 15
                        },
                        Url =
                            "http://rocklivewests3-a.akamaihd.net/17665497/t/20074887_t08ubp02qpcgamq6nfe1t196oi0wsglkyhz4ojrw.jpg"
                    },
                    User = CurrentUser
                }
            };
        }
    }
}