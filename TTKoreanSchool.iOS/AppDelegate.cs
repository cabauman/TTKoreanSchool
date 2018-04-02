using System;
using Foundation;
using Splat;
using TTKoreanSchool.iOS.Controllers;
using TTKoreanSchool.iOS.Services;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {

        public override UIWindow Window { get; set; }

        public static void ShowMessage(string title, string message, UIViewController fromViewController, Action actionForOk = null)
        {
            if(UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (obj) =>
                {
                    actionForOk?.Invoke();
                }));
                fromViewController.PresentViewController(alert, true, null);
            }
            else
            {
                new UIAlertView(title, message, null, "Ok", null).Show();
            }
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var bootstrapper = new iOSBootstrapper();
            bootstrapper.Run();
            var navService = Locator.Current.GetService<INavigationService>();
            var iosNavService = navService as NavigationService;
            Window.RootViewController = iosNavService.RootViewController;

            if(navService == null) // signed in
            {
                navService.PushPage(new HomePageViewModel());
            }
            else
            {
                navService.PushPage(new SignInPageViewModel());
            }

            Window.MakeKeyAndVisible();

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message)
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive.
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        // For iOS 9 or newer
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var openUrlOptions = new UIApplicationOpenUrlOptions(options);
            return OpenUrl(app, url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
        }

        // For iOS 8 and older
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // Google Sign-in
            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            var uri_netfx = new Uri(url.AbsoluteString);

            //SignInPageController.FacebookAuth?.OnPageLoading(uri_netfx);

            return true;
        }
    }
}