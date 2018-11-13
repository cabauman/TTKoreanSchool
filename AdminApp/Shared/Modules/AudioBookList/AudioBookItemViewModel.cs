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
using Splat;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class AudiobookItemViewModel : ReactiveObject, IAudiobookItemViewModel
    {
        private readonly ObservableAsPropertyHelper<int> _uploadProgress;
        private readonly ObservableAsPropertyHelper<string> _imageUrl;
        private readonly ObservableAsPropertyHelper<string> _audioUrl;

        private string _title;
        private bool _wasModified;
        private string _imageUrl2;

        public AudiobookItemViewModel(
            Audiobook model = null,
            IRepository<Audiobook> audiobookRepo = null,
            IFirebaseStorageService firebaseStorageService = null,
            ReactiveMediaManager mediaManager = null)
        {
            Model = model ?? new Audiobook();
            AudiobookRepo = audiobookRepo ?? Locator.Current.GetService<IRepository<Audiobook>>();
            FirebaseStorageService = firebaseStorageService ?? Locator.Current.GetService<IFirebaseStorageService>();
            MediaManager = mediaManager ?? new ReactiveMediaManager();

            Title = Model.Title;

            CancelUpload = ReactiveCommand.Create(() => Unit.Default);

            UploadImage2 = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return Observable
                        .Return(Unit.Default)
                        .Do(url => ImageUrl = "ic_delete.png");
                });
            UploadImage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".jpg", ".png" })
                        .ToObservable()
                        .Where(x => x != null)
                        .SelectMany(file => UploadFile(file, $"audiobook-images/{file.FileName}"));
                });

            UploadImage
                .Where(x => x.IsRight)
                .Select(x => x.Right)
                .Subscribe(url => ImageUrl = url);

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

            //this
            //    .WhenAnyValue(x => x.ImageUrl)
            //    .Skip(1)
            //    .Do(x => Model.ImageUrl = x)
            //    .SelectMany(_ => Model.Id != null ? AudiobookRepo.Upsert(Model) : AudiobookRepo.Add(Model))
            //    .Subscribe();

            this
                .WhenAnyValue(x => x.AudioUrl)
                .Skip(1)
                .Do(x => Model.AudioUrl = x)
                .SelectMany(_ => Model.Id != null ? AudiobookRepo.Upsert(Model) : AudiobookRepo.Add(Model))
                .Subscribe();
        }

        public Audiobook Model { get; set; }

        public ReactiveCommand<Unit, Unit> PlayAudio { get; }

        public ReactiveCommand<Unit, Unit> StopAudio { get; }

        public ReactiveCommand<Unit, Unit> UploadImage2 { get; }
        public ReactiveCommand<Unit, Either<int, string>> UploadImage { get; }

        public ReactiveCommand<Unit, string> DeleteImage { get; }

        public ReactiveCommand<Unit, Either<int, string>> UploadAudio { get; }

        public ReactiveCommand<Unit, Unit> CancelUpload { get; }

        public ReactiveCommand<Unit, string> DeleteAudio { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<Audiobook> AudiobookRepo { get; }

        public ReactiveMediaManager MediaManager { get; }

        //public string ImageUrl => _imageUrl.Value;
        public string ImageUrl
        {
            get => _imageUrl2;
            set => this.RaiseAndSetIfChanged(ref _imageUrl2, value);
        }

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

        private IObservable<Either<int, string>> UploadFile(FileData fileData, string path)
        {
            return FirebaseStorageService
                .Upload(path, fileData.GetStream())
                .TakeUntil(CancelUpload)
                .Finally(() => fileData.Dispose());
        }

        private IObservable<Unit> DeleteFile(string path)
        {
            return FirebaseStorageService
                .Delete(path);
        }
    }
}
