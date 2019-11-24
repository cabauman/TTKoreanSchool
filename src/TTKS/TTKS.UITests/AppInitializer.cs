using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace TTKS.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .InstalledApp("com.companyname.TTKS")
                    .StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}