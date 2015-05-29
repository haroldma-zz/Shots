using System.Collections.Generic;

namespace Shots.Web.Models
{
    public class BaseListResponse : BaseResponse
    {
        public List<ShotItem> Items { get; set; }
    }
}