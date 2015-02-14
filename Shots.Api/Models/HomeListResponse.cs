namespace Shots.Api.Models
{
    public class HomeListResponse : BaseListResponse
    {
        public UserInfo LoggedInUser { get; set; }
    }
}