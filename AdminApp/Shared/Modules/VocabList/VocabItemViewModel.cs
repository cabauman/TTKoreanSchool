using System;
using System.Collections.Generic;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class VocabItemViewModel : ReactiveObject, IVocabItemViewModel
    {
        //private string _ko;
        private string _romanization;
        //private string _translation;

        public VocabItemViewModel(VocabTerm model)
        {
            Model = model;
        }

        public VocabTerm Model { get; }

        public string Ko
        {
            get => Model.Ko;
            set => Model.Ko = value;
        }

        public string Romanization
        {
            get => _romanization;
            set => _romanization = value;
        }

        public string En
        {
            get => Model.Translation;
            set => Model.Translation = value;
        }

        public IList<string> ImageIds { get; set; }

        public IList<string> SentenceIds { get; set; }
    }
}
