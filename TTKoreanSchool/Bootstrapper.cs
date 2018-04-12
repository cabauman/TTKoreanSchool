﻿extern alias SplatAlias;

using Firebase.Database;
using Firebase.Database.Offline;
using I18NPortable;
using ReactiveUI;
using SplatAlias::Splat;
using System.Diagnostics;
using System.Reflection;
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

        protected abstract IViewFor<IMiniFlashcardsPageViewModel> MiniFlashcardsPage { get; }

        protected abstract IViewFor<IDetailedFlashcardsPageViewModel> DetailedFlashcardsPage { get; }

        protected abstract IViewFor<IMatchGamePageViewModel> MatchGamePage { get; }

        public void Run()
        {
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            RegisterPages();
            RegisterServices();
            RegisterViewModels();
            InitLocalization();
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
            Locator.CurrentMutable.Register(() => MiniFlashcardsPage, typeof(IViewFor<IMiniFlashcardsPageViewModel>));
            Locator.CurrentMutable.Register(() => DetailedFlashcardsPage, typeof(IViewFor<IDetailedFlashcardsPageViewModel>));
            Locator.CurrentMutable.Register(() => MatchGamePage, typeof(IViewFor<IMatchGamePageViewModel>));
        }

        protected virtual void RegisterServices()
        {
            Locator.CurrentMutable.RegisterConstant<IAccountStoreService>(new XamarinAuthAccountStoreService());
            RegisterDataServices();
        }

        private void InitLocalization()
        {
            var hostAssembly = Assembly.GetAssembly(typeof(Bootstrapper));

            I18N.Current
                .SetNotFoundSymbol("$") // Optional: when a key is not found, it will appear as $key$ (defaults to "$")
                .SetFallbackLocale("en") // Optional but recommended: locale to load in case the system locale is not supported
                .SetThrowWhenKeyNotFound(true) // Optional: Throw an exception when keys are not found (recommended only for debugging)
                .SetLogger(text => Debug.WriteLine(text)) // action to output traces
                .Init(hostAssembly); // assembly where locales live

            //I18N.Current.Locale = "es";
        }

        private void NavigateToFirstPage()
        {
            var navService = Locator.Current.GetService<INavigationService>();
            IPageViewModel page = null;

            var authService = Locator.Current.GetService<IFirebaseAuthService>();
            if(!authService.IsAuthenticated)
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
                //AuthTokenAsyncFactory = async () => await firebaseAuthService.GetFreshFirebaseToken(),
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s)
            };

            FirebaseClient firebaseClient = new FirebaseClient("https://tt-korean-academy.firebaseio.com/", firebaseOptions);

            var vocabSectionRepo = new FirebaseVocabSectionRepo(firebaseClient);
            var vocabSubsectionRepo = new FirebaseVocabSubsectionRepo(firebaseClient);
            var vocabTermRepo = new FirebaseVocabTermRepo(firebaseClient);
            var exampleSentenceRepo = new FirebaseExampleSentenceRepo(firebaseClient);
            var starredTermsRepo = new FirebaseStarredTermsRepo(firebaseClient);

            var studyContentDataService = new StudyContentDataService(
                vocabSectionRepo,
                vocabSubsectionRepo,
                vocabTermRepo,
                exampleSentenceRepo,
                starredTermsRepo);

            Locator.CurrentMutable.RegisterConstant(studyContentDataService, typeof(IStudyContentDataService));
        }
    }
}