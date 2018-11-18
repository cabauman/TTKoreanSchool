using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using TTKSCore.Config;
using TTKSCore.Extensions;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class AudiobookListViewModel : BasePageViewModel, IAudiobookListViewModel, ISupportsActivation
    {
        private readonly ObservableCollection<IAudiobookItemViewModel> _audiobookItems;
        private readonly ReadOnlyObservableCollection<IAudiobookItemViewModel> _audiobookVms;
        private readonly ISourceList<Audiobook> _audiobooks;
        private readonly ObservableAsPropertyHelper<int> _uploadProgress;

        private IAudiobookItemViewModel _selectedItem;

        public AudiobookListViewModel(
            IViewStackService viewStackService = null,
            IRepository<Audiobook> audioBookRepo = null,
            IFirebaseStorageService firebaseStorageService = null,
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
                .Transform(x => new AudiobookItemViewModel(x, mediaManager: MediaManager, confirmDelete: ConfirmDelete) as IAudiobookItemViewModel)
                .ObserveOn(mainScheduler)
                .Bind(out _audiobookVms)
                .Subscribe(_ => MakeSureItemIsSelected());

            CreateItem = ReactiveCommand.Create(
                () => _audiobooks.Add(new Audiobook()));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(
                () => AudiobookRepo.Upsert(SelectedItem.Model), canDeleteOrSaveItem);

            DeleteItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ConfirmDelete
                        .Handle(SelectedItem.Title)
                        .Where(result => result)
                        //.SelectMany(_ => DeleteFilesAndDbEntry())
                        //.ObserveOn(RxApp.MainThreadScheduler)
                        .Do(_ => _audiobooks.Remove(SelectedItem.Model))
                        .Select(_ => Unit.Default);
                },
                canDeleteOrSaveItem);

            this
                .GetIsActivated()
                .Where(isActivated => !isActivated)
                .SelectMany(_ => MediaManager.Stop().ToObservable())
                .Subscribe(_ => MediaManager.Dispose());
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

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private void MakeSureItemIsSelected()
        {
            if (SelectedItem == null && AudiobookItems.Count > 0)
            {
                SelectedItem = AudiobookItems[0];
            }
        }

        private IObservable<Unit> DeleteFilesAndDbEntry()
        {
            return Observable.Merge(
                AudiobookRepo.Delete(SelectedItem.Model.Id),
                FirebaseStorageService.Delete(Path.Combine(FirebaseStorageDirectories.AUDIOBOOK_IMAGES, SelectedItem.Model.ImageName)),
                FirebaseStorageService.Delete(Path.Combine(FirebaseStorageDirectories.AUDIOBOOK_AUDIO, SelectedItem.Model.AudioName)));
        }
    }
}
