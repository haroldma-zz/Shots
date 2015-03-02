using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class BaseResponse
    {
        private string _message;
        public Status Status { get; set; }

        [JsonProperty("msg")]
        public string Message
        {
            get { return _message ?? Response; }
            set { _message = value; }
        }

        public string Response { get; set; }
        public bool Logout { get; set; }

        [JsonProperty("server_time")]
        public int ServerTime { get; set; }

        public string Timings { get; set; }
    }

    public enum Status
    {
        Failed,
        AuthFailed,
        Success,
        BadConsumer
    }
}