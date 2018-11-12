using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.Repository;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;
using Plugin.MediaManager.Abstractions.Implementations;
using Plugin.MediaManager.Reactive;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class AudiobookItemViewModel : ReactiveObject, IAudiobookItemViewModel
    {
        private readonly ObservableAsPropertyHelper<int> _uploadProgress;
        private readonly ObservableAsPropertyHelper<string> _title;
        private readonly ObservableAsPropertyHelper<string> _imageUrl;
        private readonly ObservableAsPropertyHelper<string> _audioUrl;

        private bool _wasModified;

        public AudiobookItemViewModel(
            Audiobook model = null,
            ReactiveMediaManager mediaManager = null)
        {
            Model = model ?? new Audiobook();
            MediaManager = mediaManager ?? new ReactiveMediaManager();
            ConfirmDelete = new Interaction<string, bool>();
            DeleteImage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ConfirmDelete
                        .Handle("")
                        .Where(result => result)
                        .Select(_ => default(string));
                });

            var canPlayAudio = this.WhenAnyValue(x => x.AudioUrl).Select(x => !string.IsNullOrWhiteSpace(x));
            PlayAudio = ReactiveCommand.CreateFromObservable(
                () => MediaManager.Play(new MediaFile(AudioUrl)).ToObservable(), canPlayAudio);

            StopAudio = ReactiveCommand.CreateFromObservable(
                () => MediaManager.Stop().ToObservable(), canPlayAudio);

            DeleteImage = ReactiveCommand.Create(() => default(string));
            UploadImage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".jpg", ".png" })
                        .ToObservable()
                        .Where(x => x != null)
                        .SelectMany(file => UploadFile(file, $"audiobookImages/{file.FileName}"));
                });

            _imageUrl = DeleteImage
                .Merge(UploadImage.Where(x => x.IsRight).Select(x => x.Right))
                .ToProperty(this, x => x.ImageUrl, model.ImageUrl);

            DeleteAudio = ReactiveCommand.CreateFromObservable(
                () => DeleteFile($"audiobookAudio/{0}")
                        .SelectMany(x => AudiobookRepo.Upsert(Model))
                        .Select(_ => default(string)));

            UploadAudio = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".mp3" })
                        .ToObservable()
                        .Where(x => x != null)
                        .SelectMany(file => UploadFile(file, $"audiobookAudio/{file.FileName}"));
                });

            _audioUrl = DeleteImage
                .Merge(
                    UploadAudio
                        .Where(x => x.IsRight)
                        .SelectMany(x => AudiobookRepo.Upsert(Model).Select(_ => x.Right)))
                .ToProperty(this, x => x.AudioUrl, model.AudioUrl);

            _uploadProgress = Observable
                .Merge(UploadImage, UploadAudio)
                .Where(x => x.IsLeft)
                .Select(x => x.Left)
                .ToProperty(this, x => x.UploadProgress);

            this
                .WhenAnyValue(x => x.ImageUrl, x => x.AudioUrl, x => x.Title)
                .Subscribe(_ => WasModified = true);
        }

        public Audiobook Model { get; set; }

        public ReactiveCommand<Unit, Unit> PlayAudio { get; }

        public ReactiveCommand<Unit, Unit> StopAudio { get; }

        public ReactiveCommand<Unit, Either<int, string>> UploadImage { get; }

        public ReactiveCommand<Unit, string> DeleteImage { get; }

        public ReactiveCommand<Unit, Either<int, string>> UploadAudio { get; }

        public ReactiveCommand<Unit, Unit> CancelUpload { get; }

        public ReactiveCommand<Unit, string> DeleteAudio { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<Audiobook> AudiobookRepo { get; }

        public ReactiveMediaManager MediaManager { get; }

        public string Title => _title.Value;

        public string ImageUrl => _imageUrl.Value;

        public string AudioUrl => _audioUrl.Value;

        public int UploadProgress => _uploadProgress.Value;

        public IFirebaseStorageService FirebaseStorageService { get; }

        public bool WasModified
        {
            get => _wasModified;
            set => this.RaiseAndSetIfChanged(ref _wasModified, value);
        }

        private IObservable<Either<int, string>> UploadFile(FileData fileData, string path)
        {
            return FirebaseStorageService
                .Upload(path, fileData.GetStream())
                .TakeUntil(CancelUpload);
        }

        private IObservable<Unit> DeleteFile(string path)
        {
            return FirebaseStorageService
                .Delete(path);
        }
    }
}
