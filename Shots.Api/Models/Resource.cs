using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class Resource
    {
        [JsonProperty("auto_ts")]
        public string AutoTs { get; set; }

        public string Description { get; set; }
        public string Filter { get; set; }
        public string Front { get; set; }
        public FsVenueInfo FsVenueInfo { get; set; }
        public string Height { get; set; }
        public string Id { get; set; }
        public bool IsLike { get; set; }
        public string LikeCount { get; set; }
        public List<Like> Likes { get; set; }

        [JsonProperty("1080")]
        public string Pic1080 { get; set; }

        [JsonProperty("188")]
        public string Pic188 { get; set; }

        [JsonProperty("279")]
        public string Pic279 { get; set; }

        [JsonProperty("320")]
        public string Pic320 { get; set; }

        [JsonProperty("640")]
        public string Pic640 { get; set; }

        [JsonProperty("750")]
        public string Pic750 { get; set; }

        [JsonProperty("94")]
        public string Pic94 { get; set; }

        [JsonProperty("reactto_id")]
        public string ReactToId { get; set; }

        public string ThumbUrl { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public string WebIdent { get; set; }
        public string Width { get; set; }
    }
}