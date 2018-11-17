using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
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
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class AudiobookItemViewModel : ReactiveObject, IAudiobookItemViewModel, ISupportsActivation
    {
        private readonly ObservableAsPropertyHelper<int> _uploadProgress;
        private readonly ObservableAsPropertyHelper<string> _imageUrl;
        private readonly ObservableAsPropertyHelper<string> _audioUrl;
        private ObservableAsPropertyHelper<bool> _isPlaying;

        private string _title;
        private bool _wasModified;

        public AudiobookItemViewModel(
            Audiobook model = null,
            IRepository<Audiobook> audiobookRepo = null,
            IFirebaseStorageService firebaseStorageService = null,
            ReactiveMediaManager mediaManager = null,
            IScheduler mainScheduler = null)
        {
            Model = model ?? new Audiobook();
            AudiobookRepo = audiobookRepo ?? Locator.Current.GetService<IRepository<Audiobook>>();
            FirebaseStorageService = firebaseStorageService ?? Locator.Current.GetService<IFirebaseStorageService>();
            MediaManager = mediaManager ?? new ReactiveMediaManager();
            mainScheduler = mainScheduler ?? RxApp.MainThreadScheduler;

            Title = Model.Title;

            CancelUpload = ReactiveCommand.Create(() => Unit.Default);

            UploadImage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".jpg", ".png" })
                        .ToObservable()
                        .Where(x => x != null)
                        .SelectMany(file => UploadFile(file, $"audiobook-images/{file.FileName}"));
                });

            _imageUrl = UploadImage
                .Where(x => x.IsRight)
                .Select(x => x.Right)
                .ToProperty(this, x => x.ImageUrl, Model.ImageUrl);

            UploadAudio = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".mp3" })
                        .ToObservable()
                        .Where(x => x != null)
                        .SelectMany(file => UploadFile(file, $"audiobook-audio/{file.FileName}"));
                });

            _audioUrl = UploadAudio
                .Where(x => x.IsRight)
                .Select(x => x.Right)
                .ToProperty(this, x => x.AudioUrl, Model.AudioUrl);

            _uploadProgress = Observable
                .Merge(UploadImage, UploadAudio)
                .Where(x => x.IsLeft)
                .Select(x => x.Left)
                .ToProperty(this, x => x.UploadProgress);

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

            this.WhenActivated(
                disposables =>
                {
                _isPlaying = MediaManager
                    .IsPlaying
                    .Select(isPlaying => isPlaying && CrossMediaManager.Current.MediaQueue.Current.Url == AudioUrl)
                    .ToProperty(this, x => x.IsPlaying, scheduler: mainScheduler)
                    .DisposeWith(disposables);

                    var canPlayAudio = this.WhenAnyValue(
                        x => x.AudioUrl,
                        url => !string.IsNullOrEmpty(url));
                    //var canPlayAudio = Observable
                    //    .CombineLatest(
                    //        this.WhenAnyValue(x => x.AudioUrl).Select(x => x != null),
                    //        MediaManager.IsPlaying.StartWith(false),
                    //        (urlValid, audioPlaying) => urlValid && !audioPlaying);

                    PlayAudio = ReactiveCommand.CreateFromObservable(
                        () => MediaManager.Play(new MediaFile(AudioUrl)).ToObservable(), canPlayAudio);
                });

            StopAudio = ReactiveCommand.CreateFromObservable(
                () => MediaManager.Stop().ToObservable());
        }

        public Audiobook Model { get; set; }

        public ReactiveCommand<Unit, Unit> PlayAudio { get; private set; }

        public ReactiveCommand<Unit, Unit> StopAudio { get; }

        public bool IsPlaying => _isPlaying.Value;

        public ReactiveCommand<Unit, Either<int, string>> UploadImage { get; }

        public ReactiveCommand<Unit, string> DeleteImage { get; }

        public ReactiveCommand<Unit, Either<int, string>> UploadAudio { get; }

        public ReactiveCommand<Unit, Unit> CancelUpload { get; }

        public ReactiveCommand<Unit, string> DeleteAudio { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<Audiobook> AudiobookRepo { get; }

        public ReactiveMediaManager MediaManager { get; }

        public string ImageUrl => _imageUrl.Value;

        public string AudioUrl => _audioUrl.Value;

        public int UploadProgress => _uploadProgress.Value;

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
