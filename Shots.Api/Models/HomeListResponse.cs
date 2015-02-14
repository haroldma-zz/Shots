namespace Shots.Api.Models
{
    public class HomeListResponse : BasePageListResponse
    {
        public UserInfo LoggedInUser { get; set; }
    }
}