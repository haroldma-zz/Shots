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
}