namespace Shots.Api
{
    public static class ShotsConstants
    {
        public const string ApiBase = "api2.rocklive.com/1_04/rl/";
        public const string ApiKey = "JDoiw3g9wuFQewd*qc0x4FEvsJuzm1f5hwErh3";
        public const string ClientHeader = "WindowsPhone (ThirdParty;Zumicts)";
        public const string CredentialResouceName = "ShotsAccount";

        public const string SuggestedPath = "suggested";
        public const string UserPath = "user";
        public const string UserLoadPath = UserPath + "/load";
        public const string UserFollowingPath = UserPath + "/following";
        public const string UserFollowersPath = UserPath + "/followers";
        public const string UserLoginPath = UserPath + "/login";
        public const string UserNewPath = UserPath + "/new";
        public const string UserCheckPath = UserPath + "/check";

        public const string PostBasePath = "post";
        public const string PostByResourceIdPath = PostBasePath + "/byResource";

        public const string ListsBasePath = "lists";
        public const string ListHomePath = ListsBasePath + "/home";
        public const string ListUserPath = ListsBasePath + "/user";
        public const string ListDiscoverPath = ListsBasePath + "/discover";
        public const string ListExplorePath = ListsBasePath + "/explore";

        public const string LikeBasePath = "like";
        public const string LikeOnPath = LikeBasePath + "/on";
        public const string LikeOffPath = LikeBasePath + "/off";
    }
}