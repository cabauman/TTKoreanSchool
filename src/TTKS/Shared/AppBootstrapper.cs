using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseAuth.DotNet;
using GameCtor.RxNavigation;
using GameCtor.RxNavigation.XamForms;
using GameCtor.XamarinAuth;
using LocalStorage.XamarinEssentials;
using ReactiveUI;
using Splat;
using TTKS.Modules;
using TTKS.Core.Config;

namespace TTKS
{
    public class AppBootstrapper : ReactiveObject
    {
        private object _mainView;

        public AppBootstrapper()
        {
            RegisterServices();
            RegisterViews();
            RegisterViewModels();
        }

        public IViewShell NavigationShell { get; private set; }

        public IFirebaseAuthService FirebaseAuthService { get; private set; }

        public object MainView
        {
            get => _mainView;
            set => this.RaiseAndSetIfChanged(ref _mainView, value);
        }

        public async Task NavigateToFirstPage()
        {
            MainView = new MasterDetailViewModel(this);
            return;
            bool isAuthenticated = await FirebaseAuthService.IsAuthenticated;
            if (isAuthenticated)
            {
                MainView = new MasterDetailViewModel(this);
            }
            else
            {
                MainView = new SignInViewModel(this);
            }
        }

        private void RegisterServices()
        {

            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            NavigationShell = new ViewShell(RxApp.TaskpoolScheduler, RxApp.MainThreadScheduler, ViewLocator.Current);
            var viewStackService = new ViewStackService(NavigationShell);
            Locator.CurrentMutable.RegisterConstant(viewStackService, typeof(IViewStackService));
            Locator.CurrentMutable.Register(() => new AuthService(), typeof(IAuthService));
            FirebaseAuthService = new FirebaseAuthService(ApiKeys.FIREBASE, new SecureStorage());
            Locator.CurrentMutable.RegisterConstant(FirebaseAuthService, typeof(IFirebaseAuthService));
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(GetType().Assembly);
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which IPageViewModel we want to instantiate.
            Locator.CurrentMutable.Register(() => new HomeViewModel(this), typeof(IPageViewModel), typeof(HomeViewModel).FullName);
            Locator.CurrentMutable.Register(() => new HangulHomeViewModel(), typeof(IPageViewModel), typeof(HangulHomeViewModel).FullName);
        }
    }
}
