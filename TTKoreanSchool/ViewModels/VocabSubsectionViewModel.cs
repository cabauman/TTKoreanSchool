extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabSubsectionViewModel : IVocabSectionChildViewModel
    {
        IList<IVocabSetViewModel> VocabSets { get; }
    }

    public class VocabSubsectionViewModel : BaseViewModel, IVocabSubsectionViewModel
    {
        private VocabSectionChild _model;
        private IList<IVocabSetViewModel> _vocabSets;
        private IDialogService _dialogService;

        public VocabSubsectionViewModel(
            VocabSectionChild model,
            IList<IVocabSetViewModel> vocabSets,
            IDialogService dialogService = null)
        {
            _model = model;
            _vocabSets = vocabSets;
            _dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
        }

        public string Title
        {
            get { return _model.Title; }
        }

        public IList<IVocabSetViewModel> VocabSets
        {
            get { return _vocabSets; }
            set { this.RaiseAndSetIfChanged(ref _vocabSets, value); }
        }

        public ReactiveCommand<string, Unit> ViewChildren { get; }

        public void Selected()
        {
            _dialogService
                .DisplayActionSheet("Vocab sets", "Select a vocab set.", _vocabSets)
                .Subscribe(x => x.Selected());
        }
    }
}