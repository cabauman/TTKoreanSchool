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
using TTKoreanSchool.Modules;
using TTKSCore.Config;

namespace TTKoreanSchool
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
            FirebaseAuthService = new FirebaseAuthService(ApiKeys.FIREBASE, new LocalStorageService());
            Locator.CurrentMutable.RegisterConstant(FirebaseAuthService, typeof(IFirebaseAuthService));
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new MasterDetailPage(NavigationShell), typeof(IViewFor<MasterDetailViewModel>));
            Locator.CurrentMutable.Register(() => new AboutUsPage(), typeof(IViewFor<IAboutUsViewModel>));
            Locator.CurrentMutable.Register(() => new AudioBookListPage(), typeof(IViewFor<IAudioBookListViewModel>));
            Locator.CurrentMutable.Register(() => new AudioBookItemCell(), typeof(IViewFor<IAudioBookItemViewModel>));
            Locator.CurrentMutable.Register(() => new AudioBookPage(), typeof(IViewFor<IAudioBookViewModel>));
            Locator.CurrentMutable.Register(() => new ConjugatorPage(), typeof(IViewFor<IConjugatorViewModel>));
            Locator.CurrentMutable.Register(() => new FlashcardActivityPage(), typeof(IViewFor<IFlashcardActivityViewModel>));
            Locator.CurrentMutable.Register(() => new GrammarListPage(), typeof(IViewFor<IGrammarListViewModel>));
            Locator.CurrentMutable.Register(() => new HangulHomePage(), typeof(IViewFor<IHangulHomeViewModel>));
            Locator.CurrentMutable.Register(() => new HangulListPage(), typeof(IViewFor<IHangulListViewModel>));
            Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<IHomeViewModel>));
            Locator.CurrentMutable.Register(() => new MatchGamePage(), typeof(IViewFor<IMatchGameViewModel>));
            Locator.CurrentMutable.Register(() => new SignInPage(), typeof(IViewFor<ISignInViewModel>));
            Locator.CurrentMutable.Register(() => new VocabSetListPage(), typeof(IViewFor<IVocabSetListViewModel>));
            Locator.CurrentMutable.Register(() => new VocabTermListPage(), typeof(IViewFor<IVocabTermListViewModel>));
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which IPageViewModel we want to instantiate.
            Locator.CurrentMutable.Register(() => new HomeViewModel(this), typeof(IPageViewModel), typeof(HomeViewModel).FullName);
            Locator.CurrentMutable.Register(() => new HangulHomeViewModel(), typeof(IPageViewModel), typeof(HangulHomeViewModel).FullName);
        }
    }
}
