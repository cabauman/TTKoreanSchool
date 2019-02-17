using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using Firebase.Database;
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
        private readonly ISourceCache<StringEntity, string> _homonymCache;

        private IHomonymItemViewModel _selectedItem;

        public HomonymListViewModel(
            HomonymRepo homonymRepo = null,
            IViewStackService viewStackService = null,
            IScheduler mainScheduler = null)
                : base(viewStackService)
        {
            HomonymRepo = homonymRepo ?? Locator.Current.GetService<HomonymRepo>();
            mainScheduler = mainScheduler ?? RxApp.MainThreadScheduler;
            ConfirmDelete = new Interaction<string, bool>();
            _homonymCache = new SourceCache<StringEntity, string>(x => x.Id);

            LoadItems = ReactiveCommand.CreateFromObservable(DoLoadItems);

            var changeSet = _homonymCache
                .Connect()
                .Transform(model => new HomonymItemViewModel(model) as IHomonymItemViewModel)
                .SubscribeMany(item => item.ReceivedFocusStream.Where(flag => flag).Subscribe(flag => SelectedItem = item))
                .Publish()
                .RefCount();

            changeSet
                .ObserveOn(mainScheduler)
                .Bind(out _homonymItems)
                .Subscribe(_ => MakeSureItemIsSelected());

            CreateItem = ReactiveCommand.Create(
                () => _homonymCache.AddOrUpdate(new StringEntity() { Id = FirebaseKeyGenerator.Next() }));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(DoSaveItem, canDeleteOrSaveItem);

            DeleteItem = ReactiveCommand.CreateFromObservable(DoDeleteItem, canDeleteOrSaveItem);
        }

        public override string Title => "Homonyms";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public HomonymRepo HomonymRepo { get; }

        public IHomonymItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ReadOnlyObservableCollection<IHomonymItemViewModel> Items => _homonymItems;

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private IObservable<Unit> DoLoadItems()
        {
            return HomonymRepo
                .GetItems(false)
                .Do(x => _homonymCache.AddOrUpdate(x))
                .Select(_ => Unit.Default);
        }

        private IObservable<Unit> DoSaveItem()
        {
            return SelectedItem.Model.Id != null ?
                HomonymRepo.Upsert(SelectedItem.Model) :
                HomonymRepo.Add(SelectedItem.Model);
        }

        private IObservable<Unit> DoDeleteItem()
        {
            return ConfirmDelete
                .Handle(SelectedItem.Text)
                .Where(result => result)
                //.SelectMany(_ => DeleteFilesAndDbEntry())
                //.ObserveOn(RxApp.MainThreadScheduler)
                .Do(_ => _homonymCache.Remove(SelectedItem.Model))
                .Select(_ => Unit.Default);
        }

        private void MakeSureItemIsSelected()
        {
            if (SelectedItem == null && Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
        }
    }
}
