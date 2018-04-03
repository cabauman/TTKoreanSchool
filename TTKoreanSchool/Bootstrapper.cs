extern alias SplatAlias;

using Firebase.Database;
using Firebase.Database.Offline;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.DataAccessLayer;
using TTKoreanSchool.Services;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool
{
    public abstract class Bootstrapper
    {
        protected abstract IViewFor<ISignInPageViewModel> SignInPage { get; }

        protected abstract IViewFor<IHomePageViewModel> HomePage { get; }

        protected abstract IViewFor<IHangulZoneLandingPageViewModel> HangulZoneLandingPage { get; }

        protected abstract IViewFor<IVocabZoneLandingPageViewModel> VocabZoneLandingPage { get; }

        protected abstract IViewFor<IGrammarZoneLandingPageViewModel> GrammarZoneLandingPage { get; }

        protected abstract IViewFor<IConjugatorViewModel> ConjugatorPage { get; }

        protected abstract IViewFor<IStudentPortalPageViewModel> StudentPortalPage { get; }

        protected abstract IViewFor<IVideoFeedViewModel> VideoFeedPage { get; }

        public void Run()
        {
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            RegisterPages();
            RegisterServices();
            RegisterViewModels();
            NavigateToFirstPage();
        }

        protected abstract void RegisterViewModels();

        protected void RegisterPages()
        {
            Locator.CurrentMutable.Register(() => SignInPage, typeof(IViewFor<ISignInPageViewModel>));
            Locator.CurrentMutable.Register(() => HomePage, typeof(IViewFor<IHomePageViewModel>));
            Locator.CurrentMutable.Register(() => HangulZoneLandingPage, typeof(IViewFor<IHangulZoneLandingPageViewModel>));
            Locator.CurrentMutable.Register(() => VocabZoneLandingPage, typeof(IViewFor<IVocabZoneLandingPageViewModel>));
            Locator.CurrentMutable.Register(() => GrammarZoneLandingPage, typeof(IViewFor<IGrammarZoneLandingPageViewModel>));
            Locator.CurrentMutable.Register(() => ConjugatorPage, typeof(IViewFor<IConjugatorViewModel>));
            Locator.CurrentMutable.Register(() => StudentPortalPage, typeof(IViewFor<IStudentPortalPageViewModel>));
            Locator.CurrentMutable.Register(() => VideoFeedPage, typeof(IViewFor<IVideoFeedViewModel>));
        }

        protected virtual void RegisterServices()
        {
            Locator.CurrentMutable.RegisterConstant<IAccountStoreService>(new XamarinAuthAccountStoreService());
            RegisterDataServices();
        }

        private void NavigateToFirstPage()
        {
            var navService = Locator.Current.GetService<INavigationService>();
            IPageViewModel page = null;

            var authService = Locator.Current.GetService<IFirebaseAuthService>();
            if(authService.IsAuthenticated)
            {
                page = new HomePageViewModel();
            }
            else
            {
                page = new SignInPageViewModel();
            }

            navService.PushPage(page);
        }

        private void RegisterDataServices()
        {
            var firebaseAuthService = new FirebaseAuthService();
            Locator.CurrentMutable.RegisterConstant(firebaseAuthService, typeof(IFirebaseAuthService));

            FirebaseOptions firebaseOptions = new FirebaseOptions()
            {
                AuthTokenAsyncFactory = async () => await firebaseAuthService.GetFreshFirebaseToken(),
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s)
            };

            FirebaseClient firebaseClient = new FirebaseClient("https://tt-korean-academy.firebaseio.com/", firebaseOptions);

            var vocabSectionRepo = new FirebaseVocabSectionRepo(firebaseClient);
            var vocabSubsectionRepo = new FirebaseVocabSubsectionRepo(firebaseClient);
            var vocabTermRepo = new FirebaseVocabTermRepo(firebaseClient);
            var exampleSentenceRepo = new FirebaseExampleSentenceRepo(firebaseClient);

            var studyContentDataService = new StudyContentDataService(
                vocabSectionRepo,
                vocabSubsectionRepo,
                vocabTermRepo,
                exampleSentenceRepo);

            Locator.CurrentMutable.RegisterConstant(studyContentDataService, typeof(IStudyContentDataService));
        }
    }
}