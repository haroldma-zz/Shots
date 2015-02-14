using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class BaseListResponse : BasePageResponse
    {
        public List<ShotItem> Items { get; set; }
    }
}