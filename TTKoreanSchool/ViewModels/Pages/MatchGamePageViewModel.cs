extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Extensions;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;
using TTKoreanSchool.ViewModels.Enums;

namespace TTKoreanSchool.ViewModels
{
    public interface IMatchGamePageViewModel : IPageViewModel
    {
        int StudyPoints { get; set; }

        bool GameComplete { get; }

        List<IMatchGameCardViewModel> Cards { get; set; }

        void HandleCardSelection(IMatchGameCardViewModel viewModel);

        void SetUpNewGame();
    }

    public class MatchGamePageViewModel : BasePageViewModel, IMatchGamePageViewModel
    {
        public const int MAX_PAIRS = 6;
        public const int MAX_CARDS = MAX_PAIRS * 2;

        private readonly IStudyContentDataService _dataService;
        private readonly INavigationService _navService;
        private readonly IDialogService _dialogService;
        private readonly IAnalyticsService _analyticsService;

        private int _studyPoints;
        private int _numMatches;
        private ObservableAsPropertyHelper<bool> _gameComplete;


        public MatchGamePageViewModel(
            IStudyContentDataService dataService = null,
            INavigationService navService = null,
            IDialogService dialogService = null,
            IAnalyticsService analyticsService = null)
        {
            _dataService = dataService ?? Locator.Current.GetService<IStudyContentDataService>();
            _navService = navService ?? Locator.Current.GetService<INavigationService>();
            _dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            _analyticsService = analyticsService ?? Locator.Current.GetService<IAnalyticsService>();

            var termPool = _dataService.GetTerms();
            TermPool = new List<Term>(termPool);
            NumPairs = Math.Min(MAX_PAIRS, TermPool.Count);
            int numGameCards = NumPairs * 2;
            Cards = new List<IMatchGameCardViewModel>(numGameCards);

            _gameComplete = this.WhenAnyValue(vm => vm.NumMatches, numMatches => numMatches == NumPairs)
                .ToProperty(this, nameof(GameComplete));
            _gameComplete.ThrownExceptions.Subscribe(
                ex =>
                {
                    this.Log().ErrorException("Error", ex);
                });

            IEnumerable<IButtonViewModel> options = new ButtonViewModel[]
            {
                new ButtonViewModel("Play again", null, ReactiveCommand.Create(() => SetUpNewGame()))
            };
            this.WhenAnyValue(vm => vm.GameComplete)
                .Where(x => x == true)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => _dialogService.DisplayAlert("Complete!", null, options));

            for(int i = 0; i < MAX_CARDS; ++i)
            {
                var card = new MatchGameCardViewModel();
                Cards.Add(card);
            }
        }

        public int NumPairs { get; }

        public int StudyPoints
        {
            get { return _studyPoints; }
            set { this.RaiseAndSetIfChanged(ref _studyPoints, value); }
        }

        public int NumMatches
        {
            get { return _numMatches; }
            set { this.RaiseAndSetIfChanged(ref _numMatches, value); }
        }

        public bool GameComplete
        {
            get { return _gameComplete.Value; }
        }

        public IMatchGameCardViewModel FirstSelectedCard { get; private set; }

        public List<Term> TermPool { get; set; }

        public List<IMatchGameCardViewModel> Cards { get; set; }

        public void SetUpNewGame()
        {
            StudyPoints = 0;
            NumMatches = 0;
            TermPool.Shuffle();
            Cards.Shuffle();

            for(int i = 0; i < NumPairs; ++i)
            {
                var term = TermPool[i];
                for(int j = 0; j < 2; ++j)
                {
                    string text = j == 0 ? term.Ko : term.Translation;
                    var card = Cards[(i * 2) + j];
                    card.Id = term.Id;
                    card.Text = text;
                    card.State = MatchGameCardState.Normal;
                }
            }
        }

        public void HandleCardSelection(IMatchGameCardViewModel card)
        {
            if(card.State == MatchGameCardState.Selected)
            {
                card.State = MatchGameCardState.Normal;
                FirstSelectedCard = null;
                return;
            }

            if(FirstSelectedCard == null)
            {
                card.State = MatchGameCardState.Selected;
                FirstSelectedCard = card;
            }
            else
            {
                if(ThisCardMatchesSelectedCard(card))
                {
                    HandleMatch(card);
                }
                else
                {
                    HandleMismatch(card);
                }
            }
        }

        private bool ThisCardMatchesSelectedCard(IMatchGameCardViewModel card)
        {
            return FirstSelectedCard.Id == card.Id;
        }

        private void HandleMatch(IMatchGameCardViewModel card2)
        {
            var card1 = FirstSelectedCard;
            FirstSelectedCard = null;
            card1.State = MatchGameCardState.Match;
            card2.State = MatchGameCardState.Match;

            Observable
                .Timer(TimeSpan.FromMilliseconds(200))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    _ =>
                    {
                        card1.State = MatchGameCardState.Inactive;
                        card2.State = MatchGameCardState.Inactive;
                    });

            ++StudyPoints;
            ++NumMatches;
        }

        private void HandleMismatch(IMatchGameCardViewModel card2)
        {
            var card1 = FirstSelectedCard;
            FirstSelectedCard = null;
            card1.State = MatchGameCardState.Mismatch;
            card2.State = MatchGameCardState.Mismatch;

            Observable
                .Timer(TimeSpan.FromMilliseconds(200))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    _ =>
                    {
                        card1.State = MatchGameCardState.Normal;
                        card2.State = MatchGameCardState.Normal;
                    });

            StudyPoints = Math.Max(0, --StudyPoints);
        }
    }
}