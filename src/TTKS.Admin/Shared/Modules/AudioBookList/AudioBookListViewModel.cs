using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Acr.UserDialogs;
using DynamicData;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using Plugin.MediaManager.Abstractions.Implementations;
using Plugin.MediaManager.Reactive;
using ReactiveUI;
using Splat;
using TTKS.Core;
using TTKS.Core.Common;
using TTKS.Core.Config;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public class AudiobookListViewModel : BasePageViewModel, IAudiobookListViewModel, IActivatableViewModel
    {
        //private readonly ObservableCollection<IAudiobookItemViewModel> _audiobookItems;
        private readonly ReadOnlyObservableCollection<IAudiobookItemViewModel> _audiobookVms;
        private readonly ISourceList<Audiobook> _audiobooks;
        private readonly ObservableAsPropertyHelper<int> _uploadProgress;

        private IAudiobookItemViewModel _selectedItem;

        public AudiobookListViewModel(
            IRepository<Audiobook> audiobookRepo = null,
            IFirebaseStorageService firebaseStorageService = null,
            IUserDialogs dialogService = null,
            IScheduler mainScheduler = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            AudiobookRepo = audiobookRepo ?? Locator.Current.GetService<IRepository<Audiobook>>();
            FirebaseStorageService = firebaseStorageService ?? Locator.Current.GetService<IFirebaseStorageService>();
            DialogService = dialogService ?? UserDialogs.Instance;
            mainScheduler = mainScheduler ?? RxApp.MainThreadScheduler;
            MediaManager = new ReactiveMediaManager();
            ConfirmDelete = new Interaction<string, bool>();
            CancelUpload = ReactiveCommand.Create(() => ResetAndHideProgressDialog());
            _audiobooks = new SourceList<Audiobook>();

            LoadItems = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return AudiobookRepo
                        .GetItems(false)
                        .Do(x => _audiobooks.AddRange(x))
                        .Select(_ => Unit.Default);
                });

            var changeSet = _audiobooks
                .Connect()
                .Transform(model => new AudiobookItemViewModel(model, MediaManager, ConfirmDelete, CancelUpload, AudiobookRepo, FirebaseStorageService) as IAudiobookItemViewModel)
                .Publish()
                .RefCount();

            changeSet
                .ObserveOn(mainScheduler)
                .Bind(out _audiobookVms)
                .Subscribe(_ => MakeSureItemIsSelected());

            _uploadProgress = changeSet
                .MergeMany(GetItemUploadProgress)
                .ToProperty(this, x => x.UploadProgress);

            CreateItem = ReactiveCommand.Create(
                () => _audiobooks.Add(new Audiobook()));

            var canDeleteOrSaveItem = this
                .WhenAnyValue(x => x.SelectedItem)
                .Select(x => x != null);

            SaveItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return SelectedItem.Model.Id != null ?
                        AudiobookRepo.Upsert(SelectedItem.Model) :
                        AudiobookRepo.Add(SelectedItem.Model);
                },
                canDeleteOrSaveItem);

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
                .Subscribe(_ => CleanUp());

            ProgressDialog = DialogService.Progress(
                "Uploading...",
                () => CancelUpload.Execute().Subscribe(),
                "Cancel",
                false,
                MaskType.Black);

            this
                .WhenAnyValue(x => x.UploadProgress)
                .Where(progress => progress > 0)
                .Do(UpdateProgressDialog)
                .Where(x => x >= 100)
                .Subscribe(_ => ResetAndHideProgressDialog());

            Observable
                .Merge(
                    LoadItems.ThrownExceptions,
                    DeleteItem.ThrownExceptions,
                    changeSet.MergeMany(ListenToItemExceptions))
                .SelectMany(ex => DialogService.AlertAsync(ex.Message, "Error", "OK").ToObservable())
                .Subscribe();
        }

        public override string Title => "Audiobooks";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public ReactiveCommand<Unit, Unit> CancelUpload { get; }

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

        public IProgressDialog ProgressDialog { get; }

        public IUserDialogs DialogService { get; }

        public IReactiveMediaManager MediaManager { get; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public void CleanUp()
        {
            ProgressDialog.Dispose();
        }

        private void MakeSureItemIsSelected()
        {
            if (SelectedItem == null && AudiobookItems.Count > 0)
            {
                SelectedItem = AudiobookItems[0];
            }
        }

        private IObservable<Exception> ListenToItemExceptions(IAudiobookItemViewModel item)
        {
            return Observable
                .Merge(
                    item.UploadImage.ThrownExceptions,
                    item.UploadAudio.ThrownExceptions,
                    item.DeleteImage.ThrownExceptions,
                    item.DeleteAudio.ThrownExceptions,
                    item.PlayAudio.ThrownExceptions);
        }

        private IObservable<int> GetItemUploadProgress(IAudiobookItemViewModel item)
        {
            return Observable
                .Merge(item.UploadImage, item.UploadAudio)
                .Where(x => x.IsLeft)
                .Select(x => x.Left);
        }

        private void UpdateProgressDialog(int progress)
        {
            if (!ProgressDialog.IsShowing)
            {
                ProgressDialog.Show();
            }

            ProgressDialog.PercentComplete = progress;
        }

        private void ResetAndHideProgressDialog()
        {
            ProgressDialog.Hide();
            ProgressDialog.PercentComplete = 0;
        }

        private IObservable<Unit> DeleteFilesAndDbEntry()
        {
            return Observable.Merge(
                !string.IsNullOrWhiteSpace(SelectedItem.Model.Id) ?
                    AudiobookRepo.Delete(SelectedItem.Model.Id) :
                    Observable.Empty<Unit>(),
                !string.IsNullOrWhiteSpace(SelectedItem.Model.ImageUrl) ?
                    FirebaseStorageService.Delete(FirebaseStorageHelper.AudiobookImagePath(SelectedItem.Model.ImageName)) :
                    Observable.Empty<Unit>(),
                !string.IsNullOrWhiteSpace(SelectedItem.Model.AudioUrl) ?
                    FirebaseStorageService.Delete(FirebaseStorageHelper.AudiobookAudioPath(SelectedItem.Model.AudioName)) :
                    Observable.Empty<Unit>());
        }
    }
}
