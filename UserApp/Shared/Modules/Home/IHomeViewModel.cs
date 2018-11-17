using System.Reactive;
using ReactiveUI;

namespace TTKoreanSchool.Modules
{
    public interface IHomeViewModel
    {
        ReactiveCommand<Unit, Unit> NavigateToHangulSection { get; }

        ReactiveCommand<Unit, Unit> NavigateToVocabSection { get; }

        ReactiveCommand<Unit, Unit> NavigateToGrammarSection { get; }

        ReactiveCommand<Unit, Unit> NavigateToConjugatorSection { get; }
    }
}
