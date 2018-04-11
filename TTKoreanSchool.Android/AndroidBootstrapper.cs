using Android.Content;
using CrashlyticsKit;
using FabricSdk;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Android.Activities;
using TTKoreanSchool.Android.Services;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android
{
    public class AndroidBootstrapper : Bootstrapper
    {
        private readonly Context _context;

        public AndroidBootstrapper(Context context)
        {
            _context = context;
        }

        protected override IViewFor<ISignInPageViewModel> SignInPage => new SignInActivity();

        protected override IViewFor<IHomePageViewModel> HomePage => new MainActivity();

        protected override IViewFor<IHangulZoneLandingPageViewModel> HangulZoneLandingPage => new HangulSectionActivity();

        protected override IViewFor<IVocabZoneLandingPageViewModel> VocabZoneLandingPage => new VocabZoneActivity();

        protected override IViewFor<IGrammarZoneLandingPageViewModel> GrammarZoneLandingPage => new GrammarSectionActivity();

        protected override IViewFor<IConjugatorViewModel> ConjugatorPage => new ConjugatorActivity();

        protected override IViewFor<IStudentPortalPageViewModel> StudentPortalPage => new StudentPortalActivity();

        protected override IViewFor<IVideoFeedViewModel> VideoFeedPage => new VideoFeedActivity();

        protected override IViewFor<IMiniFlashcardsPageViewModel> MiniFlashcardsPage => new MiniFlashcardsActivity();

        protected override IViewFor<IDetailedFlashcardsPageViewModel> DetailedFlashcardsPage => throw new System.NotImplementedException();

        protected override IViewFor<IMatchGamePageViewModel> MatchGamePage => throw new System.NotImplementedException();

        protected override void RegisterViewModels()
        {
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();

            var navService = new NavigationService();
            Locator.CurrentMutable.RegisterConstant(navService, typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new AndroidLoggingService(), typeof(ILogger));

            Crashlytics.Instance.Initialize();
            Fabric.Instance.Debug = true;
            Fabric.Instance.Initialize(_context);
        }
    }
}