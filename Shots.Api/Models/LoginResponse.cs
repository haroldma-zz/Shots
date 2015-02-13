namespace Shots.Api.Models
{
    public class LoginResponse : BaseResponse
    {
        public Keys Keys { get; set; }
        public AuthenticatedUserInfo UserInfo { get; set; }
    }
}