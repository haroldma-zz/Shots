using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class ShotItem
    {
        [JsonProperty("react_to")]
        public List<ShotItem> ReactTo { get; set; }

        public Resource Resource { get; set; }
        public DateTime Time { get; set; }
        public UserInfo User { get; set; }
    }
}