using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using TTKS.Core.Common;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public class VocabItemViewModel : ReactiveObject, IVocabItemViewModel
    {
        private readonly Subject<bool> _isModifiedStream = new Subject<bool>();
        private readonly ObservableAsPropertyHelper<bool> _modified;
        private readonly ObservableAsPropertyHelper<bool> _enModified;

        private string _ko;
        private string _homonymSpecifier;
        private string _en;
        private WordClass _wordClass;
        private Transitivity _transitivity;
        private string _honorificForm;
        private string _passiveForm;
        private string _adverbForm;
        private string _notes;

        public VocabItemViewModel(VocabTerm model, Translation enTranslation)
        {
            Model = model;
            _ko = model.Ko;
            _homonymSpecifier = model.HomonymSpecifier;
            _en = enTranslation.Value;
            _honorificForm = model.HonorificForm;
            _passiveForm = model.PassiveForm;
            _adverbForm = model.AdverbForm;
            _notes = model.Notes;
            EnTranslation = enTranslation;
            Enum.TryParse(model.WordClass, out _wordClass);
            Enum.TryParse(model.Transitivity, out _transitivity);

            _modified = this
                .WhenAnyValue(
                    x => x.Ko,
                    x => x.HomonymSpecifier,
                    x => x.WordClass,
                    x => x.Transitivity,
                    x => x.HonorificForm,
                    x => x.PassiveForm,
                    x => x.AdverbForm,
                    x => x.Notes,
                    (a, b, c, d, e, f, g, h) => true)
                .Skip(1)
                .Merge(_isModifiedStream)
                .ToProperty(this, x => x.Modified);

            _enModified = this
                .WhenAnyValue(x => x.En, selector: _ => true)
                .Skip(1)
                .Merge(_isModifiedStream)
                .ToProperty(this, x => x.EnModified);

            ModifiedStream = this.WhenAnyValue(x => x.Modified);
            EnModifiedStream = this.WhenAnyValue(x => x.EnModified);
        }

        public VocabTerm Model { get; }

        public Translation EnTranslation { get; }

        public bool Modified => _modified.Value;

        public bool EnModified => _enModified.Value;

        public IObservable<bool> ModifiedStream { get; }

        public IObservable<bool> EnModifiedStream { get; }

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

        public string HomonymSpecifier
        {
            get => _homonymSpecifier;
            set => this.RaiseAndSetIfChanged(ref _homonymSpecifier, value);
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

        public void ApplyModification()
        {
            Model.Ko = Ko;
            Model.HomonymSpecifier = HomonymSpecifier;
            Model.WordClass = WordClass.ToString();
            Model.Transitivity = Transitivity.ToString();
            Model.HonorificForm = HonorificForm;
            Model.PassiveForm = PassiveForm;
            Model.AdverbForm = AdverbForm;
            Model.Notes = Notes;
            _isModifiedStream.OnNext(false);
        }

        public void ApplyEnTranslationModification()
        {
            EnTranslation.Id = Model.Id;
            EnTranslation.Value = En;
            _isModifiedStream.OnNext(false);
        }
    }
}
