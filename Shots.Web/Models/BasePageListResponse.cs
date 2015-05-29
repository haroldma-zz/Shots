using System.Collections.Generic;
using Shots.Core.Common;

namespace Shots.Web.Models
{
    public class BasePageListResponse : BasePageResponse
    {
        public IncrementalObservableCollection<ShotItem> Items { get; set; }
    }

    public class UserListResponse : BaseResponse
    {
        public List<UserInfo> Users { get; set; }
    }
}