using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shots.Api.Models
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