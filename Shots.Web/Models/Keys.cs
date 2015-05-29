using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class Keys
    {
        [JsonProperty("rl_consumer")]
        public string Consumer { get; set; }

        [JsonProperty("rl_secret")]
        public string Secret { get; set; }
    }
}