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
        protected override IViewFor<IHomePageViewModel> HomePage => throw new System.NotImplementedException();

        protected override IViewFor<ISignInPageViewModel> SignInPage => throw new System.NotImplementedException();

        protected override IViewFor<IHangulZoneLandingPageViewModel> HangulZoneLandingPage => throw new System.NotImplementedException();

        protected override IViewFor<IVocabZoneLandingPageViewModel> VocabZoneLandingPage => throw new System.NotImplementedException();

        protected override IViewFor<IGrammarZoneLandingPageViewModel> GrammarZoneLandingPage => throw new System.NotImplementedException();

        protected override IViewFor<IConjugatorViewModel> ConjugatorPage => throw new System.NotImplementedException();

        protected override IViewFor<IStudentPortalPageViewModel> StudentPortalPage => throw new System.NotImplementedException();

        protected override IViewFor<IVideoFeedViewModel> VideoFeedPage => throw new System.NotImplementedException();

        protected override void RegisterPages()
        {
            Locator.CurrentMutable.Register(() => new SignInActivity(), typeof(IViewFor<ISignInPageViewModel>));
            Locator.CurrentMutable.Register(() => new MainActivity(), typeof(IViewFor<IHomePageViewModel>));
            Locator.CurrentMutable.Register(() => new HangulSectionActivity(), typeof(IViewFor<IHangulZoneLandingPageViewModel>));
            Locator.CurrentMutable.Register(() => new VocabZoneActivity(), typeof(IViewFor<IVocabZoneLandingPageViewModel>));
            Locator.CurrentMutable.Register(() => new GrammarSectionActivity(), typeof(IViewFor<IGrammarZoneLandingPageViewModel>));
            Locator.CurrentMutable.Register(() => new ConjugatorActivity(), typeof(IViewFor<IConjugatorViewModel>));
            Locator.CurrentMutable.Register(() => new StudentPortalActivity(), typeof(IViewFor<IStudentPortalPageViewModel>));
            Locator.CurrentMutable.Register(() => new VideoFeedActivity(), typeof(IViewFor<IVideoFeedViewModel>));
        }

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