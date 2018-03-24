extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IMiniFlashcardsPageViewModel : IPageViewModel
    {
        IList<IMiniFlashcardViewModel> Terms { get; }

        ReactiveCommand<Unit, IList<IMiniFlashcardViewModel>> LoadVocabTerms { get; }

        ReactiveCommand DisplayStudyActivities { get; }
    }

    public class MiniFlashcardsPageViewModel : BasePageViewModel, IMiniFlashcardsPageViewModel
    {
        private ObservableAsPropertyHelper<IList<IMiniFlashcardViewModel>> _terms;
        private INavigationService _navService;
        private IDialogService _dialogService;
        private IStudyContentDataService _dataService;

        public MiniFlashcardsPageViewModel(
            string vocabSetId,
            INavigationService navService = null,
            IDialogService dialogService = null,
            IStudyContentDataService dataService = null)
        {
            _navService = navService ?? Locator.Current.GetService<INavigationService>();
            _dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            _dataService = dataService ?? Locator.Current.GetService<IStudyContentDataService>();
            IEnumerable<IButtonViewModel> studyActivityBtns = GetStudyActivityButtons();

            LoadVocabTerms = ReactiveCommand.CreateFromObservable(() => _dataService.GetMiniFlashcards(vocabSetId));
            LoadVocabTerms.ToProperty(this, x => x.Terms, out _terms);
            LoadVocabTerms.ThrownExceptions.Subscribe(ex => throw new Exception(ex.ToString()));

            var canExecute = this.WhenAnyValue(
                x => x.Terms,
                x => x._dataService,
                (t, n) => t != null && n != null);

            DisplayStudyActivities = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return _dialogService
                        .DisplayActionSheet("Study Activities", "Select an activity.", studyActivityBtns);
                },
                canExecute);
        }

        public IList<IMiniFlashcardViewModel> Terms
        {
            get { return _terms.Value; }
        }

        public ReactiveCommand<Unit, IList<IMiniFlashcardViewModel>> LoadVocabTerms { get; }

        public ReactiveCommand DisplayStudyActivities { get; }

        private IButtonViewModel[] GetStudyActivityButtons()
        {
            return new IButtonViewModel[]
            {
                new ButtonViewModel(
                    title: "Detailed Flashcards",
                    imageName: null,
                    command: ReactiveCommand.Create(() =>
                    {
                        _navService.PushPage(new HangulZoneLandingPageViewModel());
                    })),

                new ButtonViewModel(
                    title: "Match Game",
                    imageName: null,
                    command: ReactiveCommand.Create(() =>
                    {
                        _navService.PushPage(new VocabZoneLandingPageViewModel());
                    }))
            };
        }
    }
}