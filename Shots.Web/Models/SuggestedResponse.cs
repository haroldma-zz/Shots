using System.Collections.Generic;

namespace Shots.Web.Models
{
    public class UserInfoReponse : BaseResponse
    {
        public UserInfo UserInfo { get; set; } 
    }

    public class SuggestedResponse : BaseResponse
    {
        public List<SimpleUserInfo> Suggested { get; set; }
    }
}