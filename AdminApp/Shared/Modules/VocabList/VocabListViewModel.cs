using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            IViewStackService viewStackService = null,
            IRepository<VocabTerm> vocabTermRepo = null)
                : base(viewStackService)
        {
            VocabTermRepo = vocabTermRepo ?? Locator.Current.GetService<IRepository<VocabTerm>>();
            Items = new ObservableCollection<IVocabItemViewModel>();
            ConfirmDelete = new Interaction<string, bool>();

            CreateItem = ReactiveCommand.Create(
                () => Items.Add(new VocabItemViewModel(new VocabTerm())));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(
                () => VocabTermRepo.Upsert(SelectedItem.Model), canDeleteOrSaveItem);

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

        public ObservableCollection<IVocabItemViewModel> Items { get; }

        public IVocabItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}
