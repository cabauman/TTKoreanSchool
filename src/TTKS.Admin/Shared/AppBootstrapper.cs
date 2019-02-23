using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Firebase.Storage;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseAuth.DotNet;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.RxNavigation;
using GameCtor.RxNavigation.XamForms;
using GameCtor.XamarinAuth;
using LocalStorage.XamarinEssentials;
using ReactiveUI;
using Splat;
using TTKS.Admin.Modules;
using TTKS.Admin.Services;
using TTKS.Core;
using TTKS.Core.Config;

namespace TTKS.Admin
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
            Locator.CurrentMutable.RegisterLazySingleton(() => new FirebaseStorageService(new FirebaseStorage("tt-korean-academy.appspot.com")), typeof(IFirebaseStorageService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new StudyContentService(), typeof(StudyContentService));
            new RepoRegistrar(FirebaseAuthService, Locator.CurrentMutable);
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(GetType().Assembly);
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which IPageViewModel we want to instantiate.
            Locator.CurrentMutable.Register(() => new AudiobookListViewModel(), typeof(IPageViewModel), typeof(AudiobookListViewModel).FullName);
            Locator.CurrentMutable.Register(() => new VocabListViewModel(), typeof(IPageViewModel), typeof(VocabListViewModel).FullName);
            Locator.CurrentMutable.Register(() => new GrammarListViewModel(), typeof(IPageViewModel), typeof(GrammarListViewModel).FullName);
            Locator.CurrentMutable.Register(() => new SentenceListViewModel(), typeof(IPageViewModel), typeof(SentenceListViewModel).FullName);
            Locator.CurrentMutable.Register(() => new HomonymListViewModel(), typeof(IPageViewModel), typeof(HomonymListViewModel).FullName);
        }
    }
}
