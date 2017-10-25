using ReactiveUI;
using Splat;
using System;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IHomeViewModel : IScreenViewModel
    {
        AppSectionViewModel[] AppSections { get; }
    }

    public class HomeViewModel : BaseScreenViewModel, IHomeViewModel
    {
        public HomeViewModel()
        {
            AppSections = InitAppSections();
            NavService = Locator.Current.GetService<INavigationService>();

            var database = Locator.Current.GetService<IFirebaseDatabaseService>();
            database.LoadSentences()
                .Subscribe(
                    sentences =>
                    {
                        Console.WriteLine(sentences.Count);
                    },
                    error =>
                    {
                        this.Log().Error(error.Message);
                    });
        }

        public INavigationService NavService { get; }

        public AppSectionViewModel[] AppSections { get; }

        private AppSectionViewModel[] InitAppSections()
        {
            return new AppSectionViewModel[]
            {
                new AppSectionViewModel(
                    title: "Hangul",
                    imageName: "Icon_Hangul",
                    goToSection: ReactiveCommand.Create(() =>
                    {
                        NavService.PushScreen(new HangulZoneViewModel());
                    })),

                new AppSectionViewModel(
                    title: "Vocab",
                    imageName: "Icon_Vocab",
                    goToSection: ReactiveCommand.Create(() =>
                    {
                        NavService.PushScreen(new VocabZoneViewModel());
                    })),

                new AppSectionViewModel(
                    title: "Grammar",
                    imageName: "Icon_Grammar",
                    goToSection: ReactiveCommand.Create(() =>
                    {
                        NavService.PushScreen(new GrammarZoneViewModel());
                    })),

                new AppSectionViewModel(
                    title: "Conjugator",
                    imageName: "Icon_Conjugator",
                    goToSection: ReactiveCommand.Create(() =>
                    {
                        NavService.PushScreen(new ConjugatorViewModel());
                    })),

                new AppSectionViewModel(
                    title: "Student Portal",
                    imageName: "Icon_StudentPortal",
                    goToSection: ReactiveCommand.Create(() =>
                    {
                        NavService.PushScreen(new StudentPortalViewModel());
                    })),

                new AppSectionViewModel(
                    title: "Videos",
                    imageName: "Icon_VideoFeed",
                    goToSection: ReactiveCommand.Create(() =>
                    {
                        NavService.PushScreen(new VideoFeedViewModel());
                    }))
            };
        }
    }
}