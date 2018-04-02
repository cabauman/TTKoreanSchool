namespace TTKoreanSchool.Config
{
    public static class TwitterAuthConfig
    {
        public const string SCOPE = "email";
        public const string CONSUMER_KEY = "<CONSUMER KEY>";
        public const string CONSUMER_SECRET = "<CONSUMER SECRET>";
        public const string REQUEST_TOKEN_URL = "https://api.twitter.com/oauth/request_token";
        public const string AUTHORIZE_URL = "https://api.twitter.com/oauth/authorize";
        public const string ACCESS_TOKEN_URL = "https://api.twitter.com/oauth/access_token";
        public const string CALLBACK_URL = "http://mobile.twitter.com/home";
    }
}