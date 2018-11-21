using System;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class VocabItemViewModel : ReactiveObject, IVocabItemViewModel
    {
        private string _ko;
        private string _romanization;
        private string _translation;

        public VocabItemViewModel(VocabTerm model)
        {
            Model = model;
        }

        public VocabTerm Model { get; }

        public string Ko
        {
            get => _ko;
            set => this.RaiseAndSetIfChanged(ref _ko, value);
        }

        public string Romanization
        {
            get => _romanization;
            set => this.RaiseAndSetIfChanged(ref _romanization, value);
        }

        public string Translation
        {
            get => _translation;
            set => this.RaiseAndSetIfChanged(ref _translation, value);
        }

        public string ImageIds { get; set; }

        public string SentenceIds { get; set; }
    }
}
