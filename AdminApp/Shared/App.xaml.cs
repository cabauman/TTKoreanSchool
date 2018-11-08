using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TongTongAdmin.Views;
using Splat;
using ReactiveUI;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TongTongAdmin
{
    public partial class App : Application
    {

        public App()
        {
            LiveReload.Init();

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
