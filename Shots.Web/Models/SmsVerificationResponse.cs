using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class SmsVerificationResponse : BaseResponse
    {
        [JsonProperty("sign_up_token")]
        public string SignUpToken { get; set; }
    }
}