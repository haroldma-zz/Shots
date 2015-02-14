using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class FollowingResponse : BasePageResponse
    {
        public List<UserInfo> Following { get; set; }
    }
}