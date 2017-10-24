using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using TTKoreanSchool.Extensions;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.ViewModels
{
    public class MatchGameViewModel : BaseScreenViewModel
    {
        private const int MAX_PAIRS = 6;
        private const int MAX_CARDS = MAX_PAIRS * 2;

        private int _studyPoints;
        private bool _gameComplete;

        public MatchGameViewModel(List<Term> flashcardPool)
        {
            FlashcardPool = new List<Term>(flashcardPool);
            NumPairs = Math.Min(MAX_PAIRS, FlashcardPool.Count);
            int numGameCards = NumPairs * 2;
            GameCards = new List<MatchGameCardViewModel>(numGameCards);
            GameCardsPlusSpacers = new List<MatchGameCardViewModel>(MAX_CARDS);

            int i = 0;
            while(i < numGameCards)
            {
                var card = new MatchGameCardViewModel();
                GameCards.Add(card);
                GameCardsPlusSpacers.Add(card);
                ++i;
            }

            while(i < MAX_CARDS)
            {
                GameCardsPlusSpacers.Add(new MatchGameCardViewModel());
                ++i;
            }
        }

        public int NumPairs { get; private set; }

        public int NumMatches { get; private set; }

        public int StudyPoints
        {
            get { return _studyPoints; }
            set { this.RaiseAndSetIfChanged(ref _studyPoints, value); }
        }

        public bool GameComplete
        {
            get { return _gameComplete; }
            set { this.RaiseAndSetIfChanged(ref _gameComplete, value); }
        }

        public MatchGameCardViewModel FirstSelectedCard { get; private set; }

        public List<Term> FlashcardPool { get; set; }

        public List<MatchGameCardViewModel> GameCards { get; set; }

        public List<MatchGameCardViewModel> GameCardsPlusSpacers { get; set; }

        public void SetUpNewGame()
        {
            StudyPoints = 0;
            NumMatches = 0;
            GameComplete = false;

            FlashcardPool.Shuffle();
            GameCards.Shuffle();

            for(int i = 0; i < NumPairs; ++i)
            {
                var flashcard = FlashcardPool[i];
                for(int j = 0; j < 2; ++j)
                {
                    string text = j == 0 ? flashcard.Ko : flashcard.Translation;
                    var card = GameCards[(i * 2) + j];
                    card.Text = text;
                    card.Id = flashcard.Id;
                    card.CurState = MatchGameCardViewModel.State.NORMAL;
                }
            }
        }

        public void HandleCardSelection(MatchGameCardViewModel card)
        {
            if(card.CurState == MatchGameCardViewModel.State.SELECTED)
            {
                card.CurState = MatchGameCardViewModel.State.NORMAL;
                FirstSelectedCard = null;
                return;
            }

            if(FirstSelectedCard == null)
            {
                card.CurState = MatchGameCardViewModel.State.SELECTED;
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

        public bool ThisCardMatchesSelectedCard(MatchGameCardViewModel card)
        {
            return FirstSelectedCard.Id == card.Id;
        }

        public void HandleMatch(MatchGameCardViewModel card2)
        {
            var card1 = FirstSelectedCard;
            FirstSelectedCard = null;
            card1.CurState = MatchGameCardViewModel.State.MATCH;
            card2.CurState = MatchGameCardViewModel.State.MATCH;

            Task.Delay(200).ContinueWith(t =>
            {
                card1.CurState = MatchGameCardViewModel.State.INACTIVE;
                card2.CurState = MatchGameCardViewModel.State.INACTIVE;
            });

            ++StudyPoints;

            ++NumMatches;
            if(NumMatches == NumPairs)
            {
                GameComplete = true;
            }
        }

        public void HandleMismatch(MatchGameCardViewModel card2)
        {
            var card1 = FirstSelectedCard;
            FirstSelectedCard = null;
            card1.CurState = MatchGameCardViewModel.State.MISMATCH;
            card2.CurState = MatchGameCardViewModel.State.MISMATCH;

            Task.Delay(200).ContinueWith(t =>
            {
                card1.CurState = MatchGameCardViewModel.State.NORMAL;
                card2.CurState = MatchGameCardViewModel.State.NORMAL;
            });

            StudyPoints = Math.Max(0, --StudyPoints);
        }
    }
}