using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class BaseResponse
    {
        public Status Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("server_time")]
        public int ServerTime { get; set; }
    }

    public enum Status
    {
        Failed,
        AuthFailed,
        Success
    }
}