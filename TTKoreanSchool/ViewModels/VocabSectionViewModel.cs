using System.Collections.Generic;
using ReactiveUI;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabSectionViewModel
    {
        string Title { get; }

        IReadOnlyList<IVocabSectionChildViewModel> Children { get; }
    }

    public class VocabSectionViewModel : BaseViewModel, IVocabSectionViewModel
    {
        private VocabSection _model;

        public VocabSectionViewModel(VocabSection model, IReadOnlyList<IVocabSectionChildViewModel> children)
        {
            _model = model;
            Children = children;
        }

        public string Title
        {
            get { return _model.Title; }
        }

        public IReadOnlyList<IVocabSectionChildViewModel> Children { get; }
    }
}