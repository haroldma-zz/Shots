using Shots.Core.Common;

namespace Shots.Web.Models
{
    public class FollowersResponse : BasePageResponse
    {
        public string UserId { get; set; }
        public IncrementalObservableCollection<UserInfo> Followers { get; set; }
    }
}