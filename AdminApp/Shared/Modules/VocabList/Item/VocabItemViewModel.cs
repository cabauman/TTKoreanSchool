using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveUI;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class VocabItemViewModel : ReactiveObject, IVocabItemViewModel
    {
        private string _ko;
        private string _homonymSpecifier;
        private string _en;
        private WordClass _wordClass;
        private Transitivity _transitivity;
        private string _honorificForm;
        private string _passiveForm;
        private string _adverbForm;
        private string _notes;

        private ObservableAsPropertyHelper<bool> _modified;

        public VocabItemViewModel(VocabTerm model)
        {
            Model = model;
            _ko = model.Ko;
            _homonymSpecifier = model.HomonymSpecifier;
            _en = model.Translation;
            Enum.TryParse(model.WordClass, out _wordClass);
            Enum.TryParse(model.Transitivity, out _transitivity);
            _honorificForm = model.HonorificForm;
            _passiveForm = model.PassiveForm;
            _adverbForm = model.AdverbForm;
            _notes = model.Notes;

            _modified = this
                .WhenAnyValue(
                    x => x.Ko,
                    x => x.En,
                    x => x.HomonymSpecifier,
                    x => x.WordClass,
                    x => x.Transitivity,
                    x => x.HonorificForm,
                    x => x.PassiveForm,
                    x => x.AdverbForm,
                    x => x.Notes,
                    (a, b, c, d, e, f, g, h, i) => true)
                .Skip(1)
                .Take(1)
                .ToProperty(this, x => x.Modified);
        }

        public VocabTerm Model { get; }

        public string Ko
        {
            get => _ko;
            set => this.RaiseAndSetIfChanged(ref _ko, value);
        }

        public string HomonymSpecifier
        {
            get => _homonymSpecifier;
            set => this.RaiseAndSetIfChanged(ref _homonymSpecifier, value);
        }

        public string En
        {
            get => _en;
            set => this.RaiseAndSetIfChanged(ref _en, value);
        }

        public WordClass WordClass
        {
            get => _wordClass;
            set => this.RaiseAndSetIfChanged(ref _wordClass, value);
        }

        public Transitivity Transitivity
        {
            get => _transitivity;
            set => this.RaiseAndSetIfChanged(ref _transitivity, value);
        }

        public string HonorificForm
        {
            get => _honorificForm;
            set => this.RaiseAndSetIfChanged(ref _honorificForm, value);
        }

        public string PassiveForm
        {
            get => _passiveForm;
            set => this.RaiseAndSetIfChanged(ref _passiveForm, value);
        }

        public string AdverbForm
        {
            get => _adverbForm;
            set => this.RaiseAndSetIfChanged(ref _adverbForm, value);
        }

        public string Notes
        {
            get => _notes;
            set => this.RaiseAndSetIfChanged(ref _notes, value);
        }

        public bool Modified => _modified.Value;

        public void UpdateModel()
        {
            Model.Ko = Ko;
            Model.HomonymSpecifier = HomonymSpecifier;
            Model.Translation = En;
            Model.WordClass = WordClass.ToString();
            Model.Transitivity = Transitivity.ToString();
            Model.HonorificForm = HonorificForm;
            Model.PassiveForm = PassiveForm;
            Model.AdverbForm = AdverbForm;
            Model.Notes = Notes;
        }
    }
}
