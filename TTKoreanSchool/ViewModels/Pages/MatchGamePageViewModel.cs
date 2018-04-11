﻿extern alias SplatAlias;

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

        List<IMatchGameCardViewModel> Cards { get; }

        ReactiveCommand StartGame { get; }

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

        private readonly int _numGameCards;

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
            _numGameCards = NumPairs * 2;
            Cards = new List<IMatchGameCardViewModel>(MAX_CARDS);

            StartGame = ReactiveCommand.Create(
                () =>
                {
                    SetUpNewGame();
                });

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

            // Only create enough cards to fill the playing field, and then reuse them.
            // No need to create a view model for every vocab term.
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

        public List<Term> TermPool { get; }

        public List<IMatchGameCardViewModel> Cards { get; }

        public ReactiveCommand StartGame { get; }

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
                    card.Hidden = false;
                }
            }

            // Hide the remaining card slots if there aren't enough terms to fill the playing field.
            for(int i = _numGameCards; i < MAX_CARDS; ++i)
            {
                Cards[i].Hidden = true;
            }
        }

        public void HandleCardSelection(IMatchGameCardViewModel card)
        {
            if(card.State == MatchGameCardState.Match)
            {
                return;
            }

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
            ++StudyPoints;
            ++NumMatches;

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
                        if(card1.State == MatchGameCardState.Match)
                        {
                            card1.State = MatchGameCardState.Inactive;
                        }

                        if(card2.State == MatchGameCardState.Match)
                        {
                            card2.State = MatchGameCardState.Inactive;
                        }
                    });
        }

        private void HandleMismatch(IMatchGameCardViewModel card2)
        {
            StudyPoints = Math.Max(0, --StudyPoints);

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
                        if(card1.State == MatchGameCardState.Mismatch)
                        {
                            card1.State = MatchGameCardState.Normal;
                        }

                        if(card2.State == MatchGameCardState.Mismatch)
                        {
                            card2.State = MatchGameCardState.Normal;
                        }
                    });
        }
    }
}