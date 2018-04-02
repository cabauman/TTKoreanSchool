﻿namespace TTKoreanSchool.Config
{
    public static class GoogleAuthConfig
    {
        public const string SCOPE = "email";
        public const string AUTHORIZE_URL = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string ACCESS_TOKEN_URL = "https://www.googleapis.com/oauth2/v4/token";

        public const string CLIENT_ID_IOS = "<iOS CLIENT ID>.apps.googleusercontent.com";
        public const string REDIRECT_URL_IOS = "<BUNDLE ID>:/oauth2redirect";

        public const string CLIENT_ID_ANDROID = "<ANDROID CLIENT ID>.apps.googleusercontent.com";
        public const string REDIRECT_URL_ANDROID = "com.googleusercontent.apps.<ANDROID CLIENT ID>:/oauth2redirect";
    }
}