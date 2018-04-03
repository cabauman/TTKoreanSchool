using System;
using Android.App;
using Android.Content;
using Android.OS;
using TTKoreanSchool.Config;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "GoogleAuthInterceptor")]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
            {
                    Intent.CategoryDefault,
                    Intent.CategoryBrowsable
            },
            DataSchemes = new[]
            {
                // First part of the redirect url
                GoogleAuthConfig.DATA_SCHEME_ANDROID,
            },
            DataPaths = new[]
            {
                "/oauth2redirect"
            }
        )
    ]
    public class GoogleAuthInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

            #if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("GoogleAuthInterceptor.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            var uri = new Uri(uri_android.ToString());

            // Send the URI to the Authenticator for continuation
            SignInActivity.Authenticator?.OnPageLoading(uri);

            Finish();
        }
    }
}