using ReactiveUI;

namespace TTKoreanSchool.Modules
{
    public interface IHomeViewModel
    {
        ReactiveCommand NavigateToHangulSection { get; }

        ReactiveCommand NavigateToVocabSection { get; }

        ReactiveCommand NavigateToGrammarSection { get; }

        ReactiveCommand NavigateToConjugatorSection { get; }
    }
}
