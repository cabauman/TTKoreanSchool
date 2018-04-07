extern alias SplatAlias;

using I18NPortable;
using ReactiveUI;
using SplatAlias::Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IHomePageViewModel : IPageViewModel
    {
        IButtonViewModel[] AppSections { get; }

        ReactiveCommand Command { get; }
    }

    public class HomePageViewModel : BasePageViewModel, IHomePageViewModel
    {
        public HomePageViewModel()
        {
            AppSections = InitAppSections();
            NavService = Locator.Current.GetService<INavigationService>();

            var c = ReactiveCommand.CreateFromObservable(() =>
            {
                return Observable.Timer(TimeSpan.FromSeconds(15)).Select(x => x == 0);
            });

            c.ToProperty(this, x => x.CanExecute, out _canExecute);

            Command = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return Observable.Return(Unit.Default);
                },
                Observable.Return(false));

            //c.Execute().Subscribe();

            //var database = Locator.Current.GetService<IDataService>();
            //database.LoadSentences()
            //    .Subscribe(
            //        sentences =>
            //        {
            //            Console.WriteLine(sentences.Count);
            //        },
            //        error =>
            //        {
            //            this.Log().Error(error.Message);
            //        });
        }

        private ObservableAsPropertyHelper<bool> _canExecute;
        public bool CanExecute
        {
            get { return _canExecute.Value; }
        }

        public ReactiveCommand Command { get; }

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
                        NavService.PushPage(new VocabZoneLandingPageViewModel(), true);
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