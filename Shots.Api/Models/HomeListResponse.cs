namespace Shots.Api.Models
{
    public class HomeListResponse : BasePageResponse
    {
        public UserInfo LoggedInUser { get; set; }
    }
}