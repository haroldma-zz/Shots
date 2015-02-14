using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class UserListResponse : BasePageListResponse
    {
        public UserInfo User { get; set; }
        public List<SimpleUserInfo> Suggestions { get; set; }
    }
}