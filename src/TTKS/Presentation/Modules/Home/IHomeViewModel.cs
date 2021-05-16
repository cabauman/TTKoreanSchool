using System.Reactive;
using ReactiveUI;

namespace TTKS.Presentation.Modules
{
    public interface IHomeViewModel
    {
        ReactiveCommand<Unit, Unit> NavigateToHangulSection { get; }

        ReactiveCommand<Unit, Unit> NavigateToVocabSection { get; }

        ReactiveCommand<Unit, Unit> NavigateToGrammarSection { get; }

        ReactiveCommand<Unit, Unit> NavigateToConjugatorSection { get; }
    }
}
