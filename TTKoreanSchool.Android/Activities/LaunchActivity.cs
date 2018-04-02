using Android.App;
using Android.OS;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "LaunchActivity", MainLauncher = true, NoHistory = true, Theme = "@style/SplashTheme")]
    public class LaunchActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var bootstrapper = new AndroidBootstrapper();
            bootstrapper.Run();
        }
    }
}