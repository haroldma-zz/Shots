using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class BasePageListResponse : BasePageResponse
    {
        public List<ShotItem> Items { get; set; }
    }

    public class UserListResponse : BaseResponse
    {
        public List<UserInfo> Users { get; set; }
    }
}