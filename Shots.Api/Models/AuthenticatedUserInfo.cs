using System;
using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class SimpleUserInfo
    {
        public string Bio { get; set; }
        public string Color { get; set; }

        [JsonProperty("fname")]
        public string FirstName { get; set; }

        public string Id { get; set; }
        public bool IsFriend { get; set; }
        public bool IsRequested { get; set; }

        [JsonProperty("lname")]
        public string LastName { get; set; }

        public string Place { get; set; }
        public bool Privacy { get; set; }

        [JsonProperty("profile_photo_small")]
        public string ProfilePhotoSmall { get; set; }

        public bool Requested { get; set; }
        public string Username { get; set; }
        public string Website { get; set; }
    }

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

    public class AuthenticatedUserInfo : UserInfo
    {
        public string AppOrigin { get; set; }

        [JsonProperty("auto_ts")]
        public string AutoTs { get; set; }

        public string Dob { get; set; }
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