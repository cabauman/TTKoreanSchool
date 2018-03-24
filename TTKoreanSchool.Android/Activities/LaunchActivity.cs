using Android.App;
using Android.OS;
using Splat;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "LaunchActivity", MainLauncher = true, NoHistory = true, Theme = "@style/SplashTheme")]
    public class LaunchActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var navService = Locator.Current.GetService<INavigationService>();
            var vm = new HomePageViewModel();
            navService.PushPage(vm);
        }
    }
}