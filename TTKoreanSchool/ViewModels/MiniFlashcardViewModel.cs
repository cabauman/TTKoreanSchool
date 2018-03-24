using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IMiniFlashcardViewModel
    {
        string Ko { get; }

        string Romanization { get; }

        string Translation { get; }
    }

    public class MiniFlashcardViewModel : BaseViewModel, IMiniFlashcardViewModel
    {
        private Term _model;

        public MiniFlashcardViewModel(Term term)
        {
            _model = term;
        }

        public MiniFlashcardViewModel(Term term, string translation)
        {
        }

        public string Ko
        {
            get { return _model.Ko; }
        }

        public string Romanization
        {
            get { return _model.Romanization; }
        }

        public string Translation
        {
            get { return _model.Translation; }
        }
    }
}