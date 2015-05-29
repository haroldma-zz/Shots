using System;
using Shots.Core.Common;

namespace Shots.Web.Models
{
    public class FollowingResponse : BasePageResponse
    {
        public string UserId { get; set; }
        public DateTime Since { get; set; }
        public IncrementalObservableCollection<UserInfo> Following { get; set; }
    }
}