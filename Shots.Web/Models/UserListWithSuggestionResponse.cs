using System.Collections.Generic;

namespace Shots.Web.Models
{
    public class UserListWithSuggestionResponse : BasePageListResponse
    {
        public UserInfo User { get; set; }
        public List<SimpleUserInfo> Suggestions { get; set; }
    }
}