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
    public class SentenceListViewModel : BasePageViewModel, ISentenceListViewModel
    {
        private ISentenceItemViewModel _selectedItem;

        public SentenceListViewModel(
            IRepository<ExampleSentence> sentenceRepo = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            SentenceRepo = sentenceRepo ?? Locator.Current.GetService<IRepository<ExampleSentence>>();
            Items = new ObservableCollection<ISentenceItemViewModel>();
            ConfirmDelete = new Interaction<string, bool>();

            LoadItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return SentenceRepo
                        .GetItems(false)
                        .SelectMany(x => x)
                        .Do(x => Items.Add(new SentenceItemViewModel(x)))
                        .Select(_ => Unit.Default);
                });

            CreateItem = ReactiveCommand.Create(
                () => Items.Add(new SentenceItemViewModel(new ExampleSentence())));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return SelectedItem.Model.Id != null ?
                        SentenceRepo.Upsert(SelectedItem.Model) :
                        SentenceRepo.Add(SelectedItem.Model);
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

        public override string Title => "Sentences";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<ExampleSentence> SentenceRepo { get; }

        public ObservableCollection<ISentenceItemViewModel> Items { get; }

        public ISentenceItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}
