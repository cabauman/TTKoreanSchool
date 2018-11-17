using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using Plugin.MediaManager.Abstractions.Implementations;
using Plugin.MediaManager.Reactive;
using ReactiveUI;
using Splat;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class AudiobookListViewModel : BasePageViewModel, IAudiobookListViewModel
    {
        private readonly ObservableCollection<IAudiobookItemViewModel> _audiobookItems;
        private readonly ReadOnlyObservableCollection<IAudiobookItemViewModel> _audiobookVms;
        private readonly ISourceList<Audiobook> _audiobooks;
        private readonly ObservableAsPropertyHelper<int> _uploadProgress;

        private IAudiobookItemViewModel _selectedItem;

        public AudiobookListViewModel(
            IViewStackService viewStackService = null,
            IRepository<Audiobook> audioBookRepo = null, 
            IScheduler mainScheduler = null)
                : base(viewStackService)
        {
            AudiobookRepo = audioBookRepo ?? Locator.Current.GetService<IRepository<Audiobook>>();
            mainScheduler = mainScheduler ?? RxApp.MainThreadScheduler;
            ConfirmDelete = new Interaction<string, bool>();
            MediaManager = new ReactiveMediaManager();
            _audiobooks = new SourceList<Audiobook>();

            LoadItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return AudiobookRepo
                        .GetItems(false)
                        .Do(x => _audiobooks.AddRange(x))
                        .Select(_ => Unit.Default);
                });

            _audiobooks
                .Connect()
                .Transform(x => new AudiobookItemViewModel(x, mediaManager: MediaManager) as IAudiobookItemViewModel)
                .ObserveOn(mainScheduler)
                .Bind(out _audiobookVms)
                .Subscribe();

            CreateItem = ReactiveCommand.Create(
                () => _audiobooks.Add(new Audiobook()));

            SaveItem = ReactiveCommand.CreateFromObservable(
                () => AudiobookRepo.Upsert(SelectedItem.Model));

            DeleteItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ConfirmDelete
                        .Handle(SelectedItem.ImageUrl)
                        .Where(result => result)
                        //.SelectMany(_ => AudiobookRepository.Delete(SelectedItem.Model.Id))
                        //.ObserveOn(RxApp.MainThreadScheduler)
                        .Do(_ => _audiobooks.Remove(SelectedItem.Model))
                        .Select(_ => Unit.Default);
                });
        }

        public override string Title => "Audiobooks";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IObservable<long> ModifiedItemPulse { get; }

        public IRepository<Audiobook> AudiobookRepo { get; }

        public IFirebaseStorageService FirebaseStorageService { get; }

        public IAudiobookItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ReadOnlyObservableCollection<IAudiobookItemViewModel> AudiobookItems => _audiobookVms;



        public int UploadProgress => _uploadProgress.Value;

        public ReactiveMediaManager MediaManager { get; }
    }
}
