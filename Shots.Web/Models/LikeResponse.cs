using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class LikeResponse : BaseResponse
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        public string LikeCount { get; set; }
        public string Result { get; set; }
    }
}