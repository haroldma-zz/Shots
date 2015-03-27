using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class UserSuggestionResponse : BasePageListResponse
    {
        public UserInfo User { get; set; }
        public List<SimpleUserInfo> Suggestions { get; set; }
    }
}