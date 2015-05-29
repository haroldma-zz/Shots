using System;
using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class UserInfo : SimpleUserInfo
    {
        public bool Blocked { get; set; }

        [JsonProperty("change_time")]
        public DateTime ChangeTime { get; set; }

        public bool FollowsMe { get; set; }
        public bool IsBlocked { get; set; }
        public bool Notifying { get; set; }

        [JsonProperty("phone_verified")]
        public bool PhoneVerified { get; set; }

        [JsonProperty("108")]
        public string Pic108 { get; set; }

        [JsonProperty("132")]
        public string Pic132 { get; set; }

        [JsonProperty("200")]
        public string Pic200 { get; set; }

        [JsonProperty("36")]
        public string Pic36 { get; set; }

        [JsonProperty("72")]
        public string Pic72 { get; set; }

        [JsonProperty("profile_photo_big")]
        public string ProfilePhotoBig { get; set; }

        public bool Verified { get; set; }
    }
}