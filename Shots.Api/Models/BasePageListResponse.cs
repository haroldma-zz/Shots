using System.Collections.Generic;

namespace Shots.Api.Models
{
    public class BasePageListResponse : BasePageResponse
    {
        public List<ShotItem> Items { get; set; }
    }
}