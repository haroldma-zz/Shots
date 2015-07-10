using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class HomeListResponse : BasePageListResponse
    {
        [JsonProperty("logged_in_user")]
        public UserInfo LoggedInUser { get; set; }
    }
}