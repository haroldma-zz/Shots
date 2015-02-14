using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class UserListResponse : BasePageResponse
    {
        public UserInfo User { get; set; }
        public List<SimpleUserInfo> Suggestions { get; set; }
    }
}