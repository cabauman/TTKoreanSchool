using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using Firebase.Storage;
using Foundation;
using Plugin.Connectivity;
using Splat;
using TTKoreanSchool.iOS.Extensions;
using TTKoreanSchool.Services.Interfaces;
using System.Reactive.Concurrency;

namespace TTKoreanSchool.iOS.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly LocalStorageService _localStorage;
        private readonly StorageReference _root = Storage.DefaultInstance.GetRootReference();
        private readonly string _vocabImagePathFormat = "term-images/{0}";
        private readonly string _vocabAudioPathFormat = "term-audio/{0}";
        private readonly string _sentenceAudioPathFormat = "sentence-audio/{0}";

        private IReadOnlyDictionary<string, string> _imageUrlMap;

        public FirebaseStorageService()
        {
            var database = Locator.Current.GetService<IFirebaseDatabaseService>();
            database.LoadVocabImageUrls()
                .Subscribe(
                    imageUrlMap =>
                    {
                        _imageUrlMap = imageUrlMap;
                    });
        }

        public IObservable<string> GetDownloadUrlForVocabImage(string imageId)
        {
            if(_imageUrlMap != null && _imageUrlMap.TryGetValue(imageId, out string url))
            {
                return Observable.Return(url);
            }

            string path = string.Format(_vocabImagePathFormat, imageId);
            var imageRef = _root.GetChild(path);

            return imageRef.GetDownloadUrlRx()
                .Select(nsUrl => nsUrl.AbsoluteString);
        }

        public IObservable<string> DownloadVocabAudio(string filename, string localUrl)
        {
            var localStorage = Locator.Current.GetService<LocalStorageService>();
            localUrl = localStorage.GetVocabAudioFileUrl(filename);

            if(File.Exists(localUrl))
            {
                return Observable.Return(localUrl);
            }

            if(!CrossConnectivity.Current.IsConnected)
            {
                return Observable.Empty<string>();
            }

            string path = string.Format(_vocabAudioPathFormat, filename);
            var audioRef = _root.GetChild(path);

            return audioRef.WriteToFileRx(localUrl)
                //.SubscribeOn(Scheduler.Default)
                .Select(nsUrl => nsUrl.AbsoluteString)
                .OnErrorResumeNext(Observable.Empty<string>());
        }

        public IObservable<string> DownloadSentenceAudio(string filename, string localUrl)
        {
            string path = string.Format(_sentenceAudioPathFormat, filename);
            //var audioRef = _root.GetChild(path);

            //return audioRef.WriteToFileRx(localUrl)
            //    .Select(nsUrl => nsUrl.AbsoluteString);

            return DownloadAudio(path, localUrl);
        }

        private IObservable<string> DownloadAudio(string firebasePath, string localUrl)
        {
            if(File.Exists(localUrl))
            {
                return Observable.Return(localUrl);
            }

            if(!CrossConnectivity.Current.IsConnected)
            {
                return Observable.Empty<string>();
            }

            var audioRef = _root.GetChild(firebasePath);

            return audioRef.WriteToFileRx(localUrl)
                .Select(nsUrl => nsUrl.AbsoluteString);
        }
    }
}