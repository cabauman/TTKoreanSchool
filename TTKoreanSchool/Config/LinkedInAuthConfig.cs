namespace TTKoreanSchool.Config
{
    public static class LinkedInAuthConfig
    {
        public const string SCOPE = "r_basicprofile r_emailaddress";
        public const string CLIENT_ID = "<CLIENT ID>";
        public const string CLIENT_SECRET = "<CLIENT SECRET>";
        public const string AUTHORIZE_URL = "https://www.linkedin.com/uas/oauth2/authorization";
        public const string REDIRECT_URL = "<REDIRECT URL>";
        public const string ACCESS_TOKEN_URL = "https://www.linkedin.com/uas/oauth2/accessToken";
    }
}