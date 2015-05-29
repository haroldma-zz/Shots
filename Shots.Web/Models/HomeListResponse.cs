namespace Shots.Web.Models
{
    public class HomeListResponse : BasePageListResponse
    {
        public UserInfo LoggedInUser { get; set; }
    }
}