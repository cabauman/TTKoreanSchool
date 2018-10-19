extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Storage;
using SplatAlias::Splat;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.Services
{
    public abstract class BaseStudyContentStorageService : IStudyContentStorageService, IEnableLogger
    {
        private const string STORAGE_BUCKET = "gs://tt-korean-academy.appspot.com";

        private const string FIREBASE_PATH_VOCAB_IMAGES = "term-images";
        private const string FIREBASE_PATH_VOCAB_AUDIO = "term-audio";
        private const string FIREBASE_PATH_SENTENCE_AUDIO = "sentence-audio";

        private FirebaseStorage _storageClient;

        public BaseStudyContentStorageService()
        {
            _storageClient = new FirebaseStorage(STORAGE_BUCKET);
        }

        protected abstract string LocalRootDirectory { get; }

        public IObservable<string> GetVocabImageDownloadUrls(params string[] filenames)
        {
            FirebaseStorageReference directoryRef = _storageClient
                .Child(FIREBASE_PATH_VOCAB_IMAGES);

            return GetDownloadUrls(directoryRef, filenames);
        }

        public IObservable<IDictionary<string, string>> GetVocabAudioDownloadUrls(params string[] filenames)
        {
            FirebaseStorageReference directoryRef = _storageClient
                .Child(FIREBASE_PATH_VOCAB_AUDIO);

            return GetDownloadUrls(directoryRef, filenames);
        }

        public IObservable<KeyValuePair<string, string>> GetSentenceAudioDownloadUrls(params string[] filenames)
        {
            FirebaseStorageReference directoryRef = _storageClient
                .Child(FIREBASE_PATH_SENTENCE_AUDIO);

            return GetDownloadUrls(directoryRef, filenames);
        }

        public IObservable<string> GetDownloadUrls(FirebaseStorageReference directoryRef, params string[] filenames)
        {
            var observables = new List<IObservable<string>>();

            foreach(var filename in filenames)
            {
                var fileRef = directoryRef
                    .Child(filename)
                    .GetDownloadUrlAsync()
                    .ToObservable();

                observables.Add(fileRef);
            }

            return observables
                .Merge();

            //return observables
            //    .Zip()
            //    .Select(urls =>
            //    {
            //        return filenames.Zip(urls, (filename, url) => new { Key = filename, Value = url })
            //            .ToDictionary(x => x.Key, x => x.Value);
            //    });
        }
    }
}