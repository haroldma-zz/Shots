using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class FollowersResponse : BasePageResponse
    {
        public List<UserInfo> Followers { get; set; }
    }
}