using System;
using Android.App;
using Android.Content;
using Android.OS;
using TTKoreanSchool.Config;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "FacebookAuthInterceptor")]
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
                FacebookAuthConfig.DATA_SCHEME,
            },
            DataHosts = new[]
            {
                "authorize",
            }
            ,
            DataPaths = new[]
            {
                "/",
            }
        )
    ]
    public class FacebookAuthInterceptor : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

            #if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("FacebookAuthInterceptor.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            #endif

            var uri = new Uri(uri_android.ToString());

            // load redirect_url Page
            //SignInActivity.FacebookAuth?.OnPageLoading(uri_netfx);

            Finish();
        }
    }
}
