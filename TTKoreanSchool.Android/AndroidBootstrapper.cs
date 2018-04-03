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
        protected override IViewFor<ISignInPageViewModel> SignInPage => new SignInActivity();

        protected override IViewFor<IHomePageViewModel> HomePage => new MainActivity();

        protected override IViewFor<IHangulZoneLandingPageViewModel> HangulZoneLandingPage => new HangulSectionActivity();

        protected override IViewFor<IVocabZoneLandingPageViewModel> VocabZoneLandingPage => new VocabZoneActivity();

        protected override IViewFor<IGrammarZoneLandingPageViewModel> GrammarZoneLandingPage => new GrammarSectionActivity();

        protected override IViewFor<IConjugatorViewModel> ConjugatorPage => new ConjugatorActivity();

        protected override IViewFor<IStudentPortalPageViewModel> StudentPortalPage => new StudentPortalActivity();

        protected override IViewFor<IVideoFeedViewModel> VideoFeedPage => new VideoFeedActivity();

        protected override void RegisterViewModels()
        {
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();

            var navService = new NavigationService();
            Locator.CurrentMutable.RegisterConstant(navService, typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new AndroidLoggingService(), typeof(ILogger));
        }
    }
}