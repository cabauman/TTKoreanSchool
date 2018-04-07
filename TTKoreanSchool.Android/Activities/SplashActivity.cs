using Android.App;
using Android.OS;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "SplashActivity", MainLauncher = true, NoHistory = true, Theme = "@style/SplashTheme")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var bootstrapper = new AndroidBootstrapper();
            bootstrapper.Run();
        }
    }
}