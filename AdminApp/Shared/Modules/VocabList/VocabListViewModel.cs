using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using TongTongAdmin.Services;
using TTKSCore;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class VocabListViewModel : BasePageViewModel, IVocabListViewModel
    {
        private IVocabItemViewModel _selectedItem;
        private List<IVocabItemViewModel> _newItems;
        private List<IVocabItemViewModel> _modifiedTerms;
        private List<IVocabItemViewModel> _modifiedEnTranslations;

        public VocabListViewModel(
            IRepository<VocabTerm> vocabTermRepo = null,
            TranslationRepoFactory translationRepoFactory = null,
            StudyContentService studyContentService = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            VocabTermRepo = vocabTermRepo ?? Locator.Current.GetService<IRepository<VocabTerm>>();
            translationRepoFactory = translationRepoFactory ?? Locator.Current.GetService<TranslationRepoFactory>();
            TranslationRepo = translationRepoFactory.Create(TranslationType.Vocab, "en");
            studyContentService = studyContentService ?? Locator.Current.GetService<StudyContentService>();
            Items = new ObservableCollection<IVocabItemViewModel>();
            ConfirmDelete = new Interaction<string, bool>();
            Homonyms = studyContentService.Homonyms;

            //var o = Items
            //    .ToObservable()
            //    .SelectMany(x => x.ModifiedStream.Where(modified => modified).Select(_ => x))
            //    .Do(x => _modifiedTerms.Add(x));

            //var o2 = Items
            //    .ToObservable()
            //    .SelectMany(x => x.ModifiedEnStream.Where(modified => modified).Select(_ => x))
            //    .Do(x => _modifiedEnTranslations.Add(x));

            LoadItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    IDictionary<string, Translation> enTranslationMap = null;
                    var enTranslationStream = TranslationRepo
                        .GetItems()
                        .Do(
                            x =>
                            {
                                enTranslationMap = x.ToDictionary(translation => translation.Id);
                            })
                        .Select(_ => Unit.Default);

                    var termStream = VocabTermRepo
                        .GetItems(false)
                        .SelectMany(x => x)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(
                            term =>
                            {
                                Translation en = null;
                                enTranslationMap?.TryGetValue(term.Id, out en);
                                Items.Add(new VocabItemViewModel(term, en ?? new Translation()));
                            })
                        .Select(_ => Unit.Default);

                    return enTranslationStream.Concat(termStream);
                });

            CreateItem = ReactiveCommand.Create(
                () => Items.Add(new VocabItemViewModel(new VocabTerm(), new Translation())));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    SelectedItem.UpdateModel();
                    return SelectedItem.Model.Id != null ?
                        VocabTermRepo.Upsert(SelectedItem.Model) :
                        VocabTermRepo.Add(SelectedItem.Model);
                },
                canDeleteOrSaveItem);

            SaveAllModifiedItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    Items.Partition(x => x.Model.Id == null, out var newItems, out var existingItems);

                    newItems.Do(x => x.UpdateModel());
                    var newIitemStream = VocabTermRepo
                        .Add(newItems.Select(x => x.Model))
                        .SelectMany(_ => newItems)
                        .Do(x => x.UpdateEnTranslation())
                        .Select(x => x.EnTranslation)
                        .ToList()
                        .SelectMany(x => TranslationRepo.Upsert(x));

                    var termsQuery = existingItems
                        .Where(x => x.Modified)
                        .ToList();
                    var terms = termsQuery
                        .Do(x => x.UpdateModel())
                        .Select(x => x.Model);

                    var enTranslationsQuery = existingItems
                        .Where(x => x.En != x.EnTranslation.Value)
                        .ToList();
                    var enTranslations = enTranslationsQuery
                        .Do(x => x.UpdateEnTranslation())
                        .Select(x => x.EnTranslation);

                    //_modifiedTerms.ForEach(x => x.UpdateModel());
                    //_modifiedEnTranslations.ForEach(x => x.UpdateEnTranslation());

                    return Observable
                        .Merge(newIitemStream, VocabTermRepo.Upsert(terms), TranslationRepo.Upsert(enTranslations));
                        //.Do(_ => _modifiedTerms.Clear());
                });

            DeleteItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ConfirmDelete
                        .Handle(SelectedItem.Ko)
                        .Where(result => result)
                        //.SelectMany(_ => DeleteFilesAndDbEntry())
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .Do(_ => Items.Remove(SelectedItem))
                        .Select(_ => Unit.Default);
                },
                canDeleteOrSaveItem);
        }

        public override string Title => "Vocab";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public ReactiveCommand<Unit, Unit> SaveAllModifiedItems { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<VocabTerm> VocabTermRepo { get; }

        public IRepository<Translation> TranslationRepo { get; }

        public ObservableCollection<IVocabItemViewModel> Items { get; }

        public List<StringEntity> Homonyms { get; }

        public IVocabItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}
