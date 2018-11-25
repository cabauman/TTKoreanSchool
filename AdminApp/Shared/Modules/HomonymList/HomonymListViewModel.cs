using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class HomonymListViewModel : BasePageViewModel, IHomonymListViewModel, ISupportsActivation
    {
        private readonly ReadOnlyObservableCollection<IHomonymItemViewModel> _homonymItems;
        private readonly ISourceList<StringEntity> _homonyms;

        private IHomonymItemViewModel _selectedItem;

        public HomonymListViewModel(
            IViewStackService viewStackService = null,
            IRepository<StringEntity> homonymRepo = null,
            IScheduler mainScheduler = null)
                : base(viewStackService)
        {
            HomonymRepo = homonymRepo ?? Locator.Current.GetService<IRepository<StringEntity>>();
            mainScheduler = mainScheduler ?? RxApp.MainThreadScheduler;
            ConfirmDelete = new Interaction<string, bool>();
            _homonyms = new SourceList<StringEntity>();

            LoadItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return HomonymRepo
                        .GetItems(false)
                        .Do(x => _homonyms.AddRange(x))
                        .Select(_ => Unit.Default);
                });

            var changeSet = _homonyms
                .Connect()
                .Transform(model => new HomonymItemViewModel(model) as IHomonymItemViewModel)
                .Publish()
                .RefCount();

            changeSet
                .ObserveOn(mainScheduler)
                .Bind(out _homonymItems)
                .Subscribe(_ => MakeSureItemIsSelected());

            CreateItem = ReactiveCommand.Create(
                () => _homonyms.Add(new StringEntity()));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return SelectedItem.Model.Id != null ?
                        HomonymRepo.Upsert(SelectedItem.Model) :
                        HomonymRepo.Add(SelectedItem.Model);
                },
                canDeleteOrSaveItem);

            DeleteItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ConfirmDelete
                        .Handle(SelectedItem.Value)
                        .Where(result => result)
                        //.SelectMany(_ => DeleteFilesAndDbEntry())
                        //.ObserveOn(RxApp.MainThreadScheduler)
                        .Do(_ => _homonyms.Remove(SelectedItem.Model))
                        .Select(_ => Unit.Default);
                },
                canDeleteOrSaveItem);
        }

        public override string Title => "Homonyms";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<StringEntity> HomonymRepo { get; }

        public IHomonymItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ReadOnlyObservableCollection<IHomonymItemViewModel> Items => _homonymItems;

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private void MakeSureItemIsSelected()
        {
            if (SelectedItem == null && Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
        }
    }
}
