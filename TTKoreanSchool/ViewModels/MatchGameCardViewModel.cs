using ReactiveUI;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.ViewModels
{
    public class MatchGameCardViewModel : BaseViewModel
    {
        private string _text;
        private State _curState;

        public MatchGameCardViewModel()
        {
        }

        public MatchGameCardViewModel(Term flashcard)
        {
        }

        public enum State { NORMAL, SELECTED, MATCH, MISMATCH, INACTIVE }

        public string Id { get; set; }

        public string Text
        {
            get { return _text; }
            set { this.RaiseAndSetIfChanged(ref _text, value); }
        }

        public State CurState
        {
            get { return _curState; }
            set { this.RaiseAndSetIfChanged(ref _curState, value); }
        }
    }
}
