using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class iOSBootstrapper : Bootstrapper
    {
        protected override IViewFor<ISignInPageViewModel> SignInPage => new SignInPageController();

        protected override IViewFor<IHomePageViewModel> HomePage => new HomeController();

        protected override IViewFor<IHangulZoneLandingPageViewModel> HangulZoneLandingPage => new HangulZoneController();

        protected override IViewFor<IVocabZoneLandingPageViewModel> VocabZoneLandingPage => new VocabZoneController();

        protected override IViewFor<IGrammarZoneLandingPageViewModel> GrammarZoneLandingPage => new GrammarZoneController();

        protected override IViewFor<IConjugatorViewModel> ConjugatorPage => new ConjugatorController();

        protected override IViewFor<IStudentPortalPageViewModel> StudentPortalPage => new StudentPortalController();

        protected override IViewFor<IVideoFeedViewModel> VideoFeedPage => new VideoFeedController();

        protected override void RegisterViewModels()
        {
            Locator.CurrentMutable.Register(() => new MiniFlashcardSetController(), typeof(IViewFor<IMiniFlashcardsPageViewModel>));
            Locator.CurrentMutable.Register(() => new DetailedFlashcardSetController(), typeof(IViewFor<IDetailedFlashcardsPageViewModel>));
            Locator.CurrentMutable.Register(() => new VocabSubsectionController(), typeof(IViewFor<IVocabSubsectionViewModel>));
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();

            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new LoggingService(), typeof(ILogger));
            Locator.CurrentMutable.RegisterConstant(new DialogService(), typeof(IDialogService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new SpeechService(), typeof(ISpeechService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new AudioService(), typeof(IAudioService));
        }
    }
}