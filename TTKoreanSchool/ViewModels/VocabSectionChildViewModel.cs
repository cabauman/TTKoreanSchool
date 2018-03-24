using ReactiveUI;
using System.Reactive;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabSectionChildViewModel
    {
        string Title { get; }

        void Selected();
    }

    public abstract class VocabSectionChildViewModel : BaseViewModel, IVocabSectionChildViewModel
    {
        public string Title { get; }

        public abstract void Selected();
    }
}