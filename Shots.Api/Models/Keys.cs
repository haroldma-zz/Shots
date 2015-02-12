using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class Keys
    {
        [JsonProperty("rl_consumer")]
        public string Consumer { get; set; }

        [JsonProperty("rl_secret")]
        public string Secret { get; set; }
    }
}