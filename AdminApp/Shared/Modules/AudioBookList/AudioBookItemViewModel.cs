using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.Repository;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
//using Plugin.Media;
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
        private readonly ObservableAsPropertyHelper<string> _imageUrl2;

        private string _imageUrl;
        private string _audioUrl;
        private bool _wasModified;

        public AudiobookItemViewModel(
            Audiobook model = null,
            IRepository<Audiobook> audiobookRepo = null)
        {
            AudiobookRepo = audiobookRepo ?? Locator.Current.GetService<IRepository<Audiobook>>();
            Model = model ?? new Audiobook();
            _imageUrl = model.ImageUrl;
            _audioUrl = model.AudioUrl;

            MediaManager = new ReactiveMediaManager();
            AudioFile = !string.IsNullOrWhiteSpace(Model.AudioUrl) ? new MediaFile(Model.AudioUrl) : null;

            var canPlayAudio = this.WhenAnyValue(x => x.AudioFile).Select(x => x != null);
            PlayAudio = ReactiveCommand.CreateFromObservable(
                () => MediaManager.Play(AudioFile).ToObservable(), canPlayAudio);

            StopAudio = ReactiveCommand.CreateFromObservable(
                () => MediaManager.Stop().ToObservable());

            //UploadImage = ReactiveCommand.CreateFromObservable(
            //    () =>
            //    {
            //        return CrossFilePicker.Current.PickFile(new string[] { ".jpg", ".png" })
            //            .ToObservable()
            //            .Where(x => x != null)
            //            .SelectMany(file => UploadFile(file, $"audiobookPhotos/{file.FileName}", url => ImageUrl = url));
            //    });

            DeleteImage = ReactiveCommand.Create(() => default(string));
            UploadImage2 = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".jpg", ".png" })
                        .ToObservable()
                        .Where(x => x != null)
                        .SelectMany(file => UploadFile(file, $"audiobookPhotos/{file.FileName}"));
                });
            //_imageUrl2 = UploadImage2
            //    .Where(x => x.IsRight)
            //    .Select(x => x.Right)
            //    .Merge(DeleteImage)
            //    .ToProperty(this, x => x.ImageUrl2, model.ImageUrl);
            _imageUrl2 = DeleteImage
                .Merge(UploadImage2.Where(x => x.IsRight).Select(x => x.Right))
                .ToProperty(this, x => x.ImageUrl2, model.ImageUrl);

            UploadAudio = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFilePicker.Current.PickFile(new string[] { ".mp3" })
                        .ToObservable()
                        .Where(x => x != null)
                        .SelectMany(file => UploadFile(file, $"audiobookAudio/{file.FileName}", url => AudioUrl = url));
                });

            _uploadProgress = Observable
                .Merge(UploadImage, UploadAudio)
                .ToProperty(this, x => x.UploadProgress);

            this
                .WhenAnyValue(x => x.ImageUrl, x => x.AudioUrl, x => x.Title)
                .Subscribe(_ => WasModified = true);
        }

        public Audiobook Model { get; set; }

        public ReactiveCommand<Unit, Unit> PlayAudio { get; }

        public ReactiveCommand<Unit, Unit> StopAudio { get; }

        public ReactiveCommand<Unit, int> UploadImage { get; }
        public ReactiveCommand<Unit, string> DeleteImage { get; }
        public ReactiveCommand<Unit, Either<int, string>> UploadImage2 { get; }

        public ReactiveCommand<Unit, int> UploadAudio { get; }

        public ReactiveMediaManager MediaManager { get; }

        public IMediaFile AudioFile { get; set; }

        public string Title { get; set; }

        public string ImageUrl2 => _imageUrl2.Value;
        public string ImageUrl
        {
            get => _imageUrl;
            set => this.RaiseAndSetIfChanged(ref _imageUrl, value);
        }

        public string AudioUrl
        {
            get => _audioUrl;
            set => this.RaiseAndSetIfChanged(ref _audioUrl, value);
        }

        public bool WasModified
        {
            get => _wasModified;
            set => this.RaiseAndSetIfChanged(ref _wasModified, value);
        }

        public IRepository<Audiobook> AudiobookRepo { get; }



        public IFirebaseStorageService FirebaseStorageService { get; }

        public int UploadProgress => _uploadProgress.Value;

        //private IObservable<int> Save(string downloadUrl)
        //{
        //    return AudiobookRepo
        //        .Upsert(Model)
        //        .Select(_ => 100);
        //}

        private IObservable<Either<int, string>> UploadFile(FileData fileData, string path)
        {
            return FirebaseStorageService
                .Upload(path, fileData.GetStream());
        }

        private IObservable<int> UploadFile(FileData fileData, string path, Action<string> onComplete)
        {
            return FirebaseStorageService
                .Upload(path, fileData.GetStream())
                .Select(
                    x =>
                    {
                        if (x.IsLeft)
                        {
                            return x.Left;
                        }
                        else
                        {
                            fileData.Dispose();
                            onComplete?.Invoke(x.Right);
                            return 100;
                        }
                    });
        }
    }
}
