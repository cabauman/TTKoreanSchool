using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Firebase.Database;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using TTKS.Admin.Services;
using TTKS.Core;
using TTKS.Core.Common;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public class VocabListViewModel : BasePageViewModel, IVocabListViewModel
    {
        private IVocabItemViewModel _selectedItem;
        private ReadOnlyObservableCollection<StringEntity> _homonyms;
        private IDictionary<string, IVocabItemViewModel> _modifiedTermMap = new Dictionary<string, IVocabItemViewModel>();
        private IDictionary<string, IVocabItemViewModel> _modifiedEnTranslationMap = new Dictionary<string, IVocabItemViewModel>();

        public VocabListViewModel(
            VocabTermRepo vocabTermRepo = null,
            TranslationRepoFactory translationRepoFactory = null,
            StudyContentService studyContentService = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            VocabTermRepo = vocabTermRepo ?? Locator.Current.GetService<VocabTermRepo>();
            translationRepoFactory = translationRepoFactory ?? Locator.Current.GetService<TranslationRepoFactory>();
            TranslationRepo = translationRepoFactory.Create(TranslationType.Vocab, "en");
            studyContentService = studyContentService ?? Locator.Current.GetService<StudyContentService>();
            Items = new ObservableCollection<IVocabItemViewModel>();
            ConfirmDelete = new Interaction<string, bool>();

            studyContentService
                .GetHomonymChangeSet()
                .Subscribe(x => _homonyms = x);

            SubscribeToItemModifications();

            LoadItems = ReactiveCommand.CreateFromObservable(DoLoadItems);

            CreateItem = ReactiveCommand.Create(DoCreateItem);

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(DoSaveItem, canDeleteOrSaveItem);

            SaveAllModifiedItems = ReactiveCommand.CreateFromObservable(DoSaveAllModifiedItems);

            DeleteItem = ReactiveCommand.CreateFromObservable(DoDeleteItem, canDeleteOrSaveItem);
        }

        public override string Title => "Vocab";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public ReactiveCommand<Unit, Unit> SaveAllModifiedItems { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public VocabTermRepo VocabTermRepo { get; }

        public IRepository<Translation> TranslationRepo { get; }

        public ObservableCollection<IVocabItemViewModel> Items { get; }

        public ReadOnlyObservableCollection<StringEntity> Homonyms => _homonyms;

        public IVocabItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        private IObservable<Unit> DoLoadItems()
        {
            return Observable
                .CombineLatest(
                    VocabTermRepo.GetItems(true),
                    TranslationRepo.GetItems(true),
                    (x, y) => x.Zip(y, (term, translation) => new VocabItemViewModel(term, translation)))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(x => Items.AddRange(x))
                .Select(_ => Unit.Default);
        }

        private void DoCreateItem()
        {
            Items.Add(new VocabItemViewModel(new VocabTerm() { Id = FirebaseKeyGenerator.Next() }, new Translation()));
        }

        private IObservable<Unit> DoSaveItem()
        {
            SelectedItem.ApplyModification();
            return SelectedItem.Model.Id != null ?
                VocabTermRepo.Upsert(SelectedItem.Model) :
                VocabTermRepo.Add(SelectedItem.Model);
        }

        private IObservable<Unit> DoSaveAllModifiedItems()
        {
            var terms = _modifiedTermMap.Values
                .Do(x => x.ApplyModification())
                .Select(x => x.Model);

            var enTranslations = _modifiedEnTranslationMap.Values
                .Do(x => x.ApplyEnTranslationModification())
                .Select(x => x.EnTranslation);

            return Observable
                .Merge(VocabTermRepo.Upsert(terms), TranslationRepo.Upsert(enTranslations))
                .Do(
                    _ =>
                    {
                        _modifiedTermMap.Clear();
                        _modifiedEnTranslationMap.Clear();
                    });
        }

        private IObservable<Unit> DoDeleteItem()
        {
            return ConfirmDelete
                .Handle(SelectedItem.Ko)
                .Where(result => result)
                //.SelectMany(_ => DeleteFilesAndDbEntry())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(_ => Items.Remove(SelectedItem))
                .Select(_ => Unit.Default);
        }

        private void SubscribeToItemModifications()
        {
            Items.ToObservableChangeSet()
                .SubscribeMany(
                    x =>
                    {
                        return x.ModifiedStream
                            .Where(modified => modified)
                            .Subscribe(_ => _modifiedTermMap.Add(x.Model.Id, x));
                    })
                .OnItemRemoved(
                    x =>
                    {
                        if (_modifiedTermMap.ContainsKey(x.Model.Id))
                        {
                            _modifiedTermMap.Remove(x.Model.Id);
                        }
                    })
                .Subscribe();

            Items.ToObservableChangeSet()
                .SubscribeMany(
                    x =>
                    {
                        return x.EnModifiedStream
                            .Where(modified => modified)
                            .Subscribe(_ => _modifiedEnTranslationMap.Add(x.Model.Id, x));
                    })
                .OnItemRemoved(
                    x =>
                    {
                        if (_modifiedEnTranslationMap.ContainsKey(x.Model.Id))
                        {
                            _modifiedEnTranslationMap.Remove(x.Model.Id);
                        }
                    })
                .Subscribe();
        }
    }
}
