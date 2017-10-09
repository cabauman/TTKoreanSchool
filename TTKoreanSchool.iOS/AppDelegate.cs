using CoreGraphics;
using Foundation;
using ReactiveUI;
using Splat;
using TTKoreanSchool.iOS.Controllers;
using TTKoreanSchool.iOS.Services;
using TTKoreanSchool.Services;
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

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Window.RootViewController = new UINavigationController();

            RegisterServices();
            var navService = Locator.Current.GetService<INavigationService>();
            navService.PushScreen(new HomeViewModel());

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

        private void RegisterServices()
        {
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            Locator.CurrentMutable.Register(() => new HomeController(HomeController.GetLayout()), typeof(IViewFor<IHomeViewModel>));
            Locator.CurrentMutable.Register(() => new HangulZoneController(), typeof(IViewFor<IHangulZoneViewModel>));
            Locator.CurrentMutable.Register(() => new VocabZoneController(), typeof(IViewFor<IVocabZoneViewModel>));
            Locator.CurrentMutable.Register(() => new GrammarZoneController(), typeof(IViewFor<IGrammarZoneViewModel>));
            Locator.CurrentMutable.Register(() => new ConjugatorController(), typeof(IViewFor<IConjugatorViewModel>));
            Locator.CurrentMutable.Register(() => new StudentPortalController(), typeof(IViewFor<IStudentPortalViewModel>));
            Locator.CurrentMutable.Register(() => new VideoFeedController(), typeof(IViewFor<IVideoFeedViewModel>));

            var navService = new NavigationService(Window.RootViewController);
            Locator.CurrentMutable.RegisterConstant(navService, typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new LoggingService(), typeof(ILogger));
        }
    }
}