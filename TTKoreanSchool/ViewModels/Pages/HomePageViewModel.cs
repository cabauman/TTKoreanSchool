extern alias SplatAlias;

using System;
using System.Reactive;
using System.Reactive.Linq;
using I18NPortable;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IHomePageViewModel : IPageViewModel
    {
        IButtonViewModel[] AppSections { get; }

        IReadOnlyReactiveList<IButtonViewModel> AppSections2 { get; }
    }

    public class HomePageViewModel : BasePageViewModel, IHomePageViewModel
    {
        public HomePageViewModel(INavigationService navService = null)
        {
            AppSections = InitAppSections();
            NavService = navService ?? Locator.Current.GetService<INavigationService>();
        }

        public INavigationService NavService { get; }

        public IButtonViewModel[] AppSections { get; }

        private IButtonViewModel[] InitAppSections()
        {
            return new IButtonViewModel[]
            {
                new ButtonViewModel(
                    title: "Greeting.Title".Translate(), // "Hangul",
                    imageName: "Icon_Hangul",
                    command: ReactiveCommand.Create(() =>
                    {
                        NavService.PushPage(new HangulZoneLandingPageViewModel());
                    })),

                new ButtonViewModel(
                    title: "Vocab",
                    imageName: "Icon_Vocab",
                    command: ReactiveCommand.Create(() =>
                    {
                        NavService.PushPage(new VocabZoneLandingPageViewModel());
                    })),

                new ButtonViewModel(
                    title: "Grammar",
                    imageName: "Icon_Grammar",
                    command: ReactiveCommand.Create(() =>
                    {
                        NavService.PushPage(new GrammarZoneLandingPageViewModel());
                    })),

                new ButtonViewModel(
                    title: "Conjugator",
                    imageName: "Icon_Conjugator",
                    command: ReactiveCommand.Create(() =>
                    {
                        NavService.PushPage(new ConjugatorViewModel());
                    })),

                new ButtonViewModel(
                    title: "Student Portal",
                    imageName: "Icon_StudentPortal",
                    command: ReactiveCommand.Create(() =>
                    {
                        NavService.PushPage(new StudentPortalPageViewModel());
                    })),

                new ButtonViewModel(
                    title: "Videos",
                    imageName: "Icon_VideoFeed",
                    command: ReactiveCommand.Create(() =>
                    {
                        NavService.PushPage(new VideoFeedViewModel());
                    }))
            };
        }
    }
}