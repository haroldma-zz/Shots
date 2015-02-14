using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class Like
    {
        public string Fname { get; set; }
        public string Lname { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public string Username { get; set; }
    }
}