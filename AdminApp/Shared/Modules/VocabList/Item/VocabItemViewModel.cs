using System;
using System.Collections.Generic;
using ReactiveUI;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class VocabItemViewModel : ReactiveObject, IVocabItemViewModel
    {
        private string _ko;
        private string _en;

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

        public string En
        {
            get => _en;
            set => this.RaiseAndSetIfChanged(ref _en, value);
        }

        public string HomonymSpecifier { get; set; }

        public string WordClass { get; set; }

        public string Transitivity { get; set; }

        public string HonorificForm { get; set; }

        public string PassiveForm { get; set; }

        public string AdverbForm { get; set; }

        public string Notes { get; set; }

        public IList<string> ImageIds { get; set; }

        public IList<string> SentenceIds { get; set; }
    }
}
