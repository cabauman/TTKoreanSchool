using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class VocabListViewModel : BasePageViewModel, IVocabListViewModel
    {
        private IVocabItemViewModel _selectedItem;

        public VocabListViewModel(
            IRepository<VocabTerm> vocabTermRepo = null,
            TranslationRepoFactory translationRepoFactory = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            VocabTermRepo = vocabTermRepo ?? Locator.Current.GetService<IRepository<VocabTerm>>();
            translationRepoFactory = translationRepoFactory ?? Locator.Current.GetService<TranslationRepoFactory>();
            TranslationRepo = translationRepoFactory.Create(TranslationType.Vocab, "en");
            Items = new ObservableCollection<IVocabItemViewModel>();
            ConfirmDelete = new Interaction<string, bool>();

            LoadItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    //var e = Enumerable.Range(0, 10).Select(x => new VocabTerm());
                    //var translations = TranslationRepo
                    //    .GetItems()
                    //    .SelectMany(x => x.Join(e, t => t.Id, other => other.Id,
                    //        (a, b) =>
                    //        {
                    //            b.Translation = a.Value;
                    //            return b;
                    //        }));

                    return VocabTermRepo
                        .GetItems(false)
                        .SelectMany(x => x)
                        .Do(x => Items.Add(new VocabItemViewModel(x)))
                        .Select(_ => Unit.Default);
                });

            CreateItem = ReactiveCommand.Create(
                () => Items.Add(new VocabItemViewModel(new VocabTerm())));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return SelectedItem.Model.Id != null ?
                        VocabTermRepo.Upsert(SelectedItem.Model) :
                        VocabTermRepo.Add(SelectedItem.Model);
                },
                canDeleteOrSaveItem);

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

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<VocabTerm> VocabTermRepo { get; }

        public IRepository<Translation> TranslationRepo { get; }

        public ObservableCollection<IVocabItemViewModel> Items { get; }

        public IVocabItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}
