using System;
using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class AuthenticatedUserInfo : UserInfo
    {
        public string AppOrigin { get; set; }

        [JsonProperty("auto_ts")]
        public string AutoTs { get; set; }

        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Hometown { get; set; }

        [JsonProperty("instagram_id")]
        public string InstagramId { get; set; }

        [JsonProperty("isprivate")]
        public bool IsPrivate { get; set; }

        public string LastLogin { get; set; }

        [JsonProperty("last_upload_time")]
        public DateTime LastUploadTime { get; set; }

        [JsonProperty("new_loc")]
        public int NewLoc { get; set; }

        public string Phone { get; set; }

        [JsonProperty("phone_country_code")]
        public string PhoneCountryCode { get; set; }

        [JsonProperty("pic_ts")]
        public string PicTs { get; set; }

        public int Profile { get; set; }

        [JsonProperty("signup_time")]
        public DateTime SignupTime { get; set; }

        [JsonProperty("tumblr_usr")]
        public string TumblrUsr { get; set; }

        [JsonProperty("twitter_id")]
        public string TwitterId { get; set; }
    }
}