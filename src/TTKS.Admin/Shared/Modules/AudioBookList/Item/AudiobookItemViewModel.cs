using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.Repository;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions.Implementations;
using Plugin.MediaManager.Reactive;
using ReactiveUI;
using Splat;
using TTKS.Core.Config;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public class AudiobookItemViewModel : ReactiveObject, IAudiobookItemViewModel, ISupportsActivation
    {
        private readonly ObservableAsPropertyHelper<string> _imageUrl;
        private readonly ObservableAsPropertyHelper<string> _audioUrl;
        private ObservableAsPropertyHelper<bool> _isPlaying;

        private string _title;
        private bool _wasModified;

        public AudiobookItemViewModel(
            Audiobook model,
            IReactiveMediaManager mediaManager,
            Interaction<string, bool> confirmDelete,
            ReactiveCommand<Unit, Unit> cancelUpload,
            IRepository<Audiobook> audiobookRepo = null,
            IFirebaseStorageService firebaseStorageService = null,
            IScheduler mainScheduler = null)
        {
            Model = model;
            MediaManager = mediaManager;
            CancelUpload = cancelUpload;
            AudiobookRepo = audiobookRepo ?? Locator.Current.GetService<IRepository<Audiobook>>();
            FirebaseStorageService = firebaseStorageService ?? Locator.Current.GetService<IFirebaseStorageService>();
            mainScheduler = mainScheduler ?? RxApp.MainThreadScheduler;

            Title = Model.Title;

            UploadImage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".jpg", ".png" })
                        .ToObservable()
                        .Where(x => x != null)
                        .Do(file => Model.ImageName = file.FileName)
                        .SelectMany(file => UploadFile(file, $"{FirebaseStorageHelper.AUDIOBOOK_IMAGES}/{file.FileName}"));
                });

            var canDeleteImage = this
                .WhenAnyValue(x => x.ImageUrl)
                .Select(url => !string.IsNullOrWhiteSpace(url));

            DeleteImage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return confirmDelete
                        .Handle(Model.ImageName)
                        .Where(result => result)
                        .SelectMany(_ => DeleteFile($"{FirebaseStorageHelper.AUDIOBOOK_IMAGES}/{Model.ImageName}"))
                        .Do(_ => Model.ImageName = default(string))
                        .Select(_ => default(string));
                },
                canDeleteImage);

            _imageUrl = UploadImage
                .Where(x => x.IsRight)
                .Select(x => x.Right)
                .Merge(DeleteImage)
                .ToProperty(this, x => x.ImageUrl, Model.ImageUrl);

            UploadAudio = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".mp3" })
                        .ToObservable()
                        .Where(x => x != null)
                        .Do(file => Model.AudioName = file.FileName)
                        .SelectMany(file => UploadFile(file, $"{FirebaseStorageHelper.AUDIOBOOK_AUDIO}/{file.FileName}"));
                });

            var canDeleteAudio = this
                .WhenAnyValue(x => x.AudioUrl)
                .Select(url => !string.IsNullOrWhiteSpace(url));

            DeleteAudio = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return confirmDelete
                        .Handle(Model.AudioName)
                        .Where(result => result)
                        .SelectMany(_ => DeleteFile($"{FirebaseStorageHelper.AUDIOBOOK_AUDIO}/{Model.AudioName}"))
                        .Do(file => Model.AudioName = default(string))
                        .Select(_ => default(string));
                },
                canDeleteAudio);

            _audioUrl = UploadAudio
                .Where(x => x.IsRight)
                .Select(x => x.Right)
                .Merge(DeleteAudio)
                .ToProperty(this, x => x.AudioUrl, Model.AudioUrl);

            this
                .WhenAnyValue(x => x.ImageUrl)
                .Skip(1)
                .Do(x => Model.ImageUrl = x)
                .SelectMany(_ => Model.Id != null ? AudiobookRepo.Upsert(Model) : AudiobookRepo.Add(Model))
                .Subscribe();

            this
                .WhenAnyValue(x => x.AudioUrl)
                .Skip(1)
                .Do(x => Model.AudioUrl = x)
                .SelectMany(_ => Model.Id != null ? AudiobookRepo.Upsert(Model) : AudiobookRepo.Add(Model))
                .Subscribe();

            _isPlaying = MediaManager
                .IsPlaying
                .Select(isPlaying => isPlaying && CrossMediaManager.Current.MediaQueue.Current.Url == AudioUrl)
                .ToProperty(this, x => x.IsPlaying, deferSubscription: true, scheduler: mainScheduler);

            var canPlayAudio = this.WhenAnyValue(
                x => x.AudioUrl,
                url => !string.IsNullOrEmpty(url));

            PlayAudio = ReactiveCommand.CreateFromObservable(
                () => MediaManager.Play(new MediaFile(AudioUrl)).ToObservable(), canPlayAudio);

            StopAudio = ReactiveCommand.CreateFromObservable(
                () => MediaManager.Stop().ToObservable());
        }

        public Audiobook Model { get; set; }

        public ReactiveCommand<Unit, Unit> PlayAudio { get; private set; }

        public ReactiveCommand<Unit, Unit> StopAudio { get; }

        public ReactiveCommand<Unit, Either<int, string>> UploadImage { get; }

        public ReactiveCommand<Unit, string> DeleteImage { get; }

        public ReactiveCommand<Unit, Either<int, string>> UploadAudio { get; }

        public ReactiveCommand<Unit, Unit> CancelUpload { get; }

        public ReactiveCommand<Unit, string> DeleteAudio { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<Audiobook> AudiobookRepo { get; }

        public IReactiveMediaManager MediaManager { get; }

        public bool IsPlaying => _isPlaying != null ? _isPlaying.Value : false;

        public string ImageUrl => _imageUrl?.Value;

        public string AudioUrl => _audioUrl?.Value;

        public IFirebaseStorageService FirebaseStorageService { get; }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        public bool WasModified
        {
            get => _wasModified;
            set => this.RaiseAndSetIfChanged(ref _wasModified, value);
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        private IObservable<Either<int, string>> UploadFile(FileData fileData, string path)
        {
            return FirebaseStorageService
                .Upload(path, fileData.GetStream())
                .TakeUntil(CancelUpload)
                .Finally(
                    () =>
                    {
                        fileData.Dispose();
                    });
        }

        private IObservable<Unit> DeleteFile(string path)
        {
            return FirebaseStorageService
                .Delete(path);
        }
    }
}
