using System;
using System.Collections.Generic;
using System.Reactive;
using Firebase.Database;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseStudyContentDownloadUrlRepo : FirebaseRepo<string>, IStudyContentDownloadUrlRepo
    {
        private readonly ChildQuery _downloadUrlsRef;
        private readonly string _vocabImageUrlDirectory = "term-images";

        public FirebaseStudyContentDownloadUrlRepo(FirebaseClient client)
        {
            _downloadUrlsRef = client.Child("download-urls");
        }

        public IObservable<string> ReadAll(string directory)
        {
            ChildQuery childQuery = _downloadUrlsRef
                .Child(directory);

            return ReadAll(childQuery);
        }

        public IObservable<string> ReadVocabImageUrl(string fileId)
        {
            ChildQuery childQuery = _downloadUrlsRef
                .Child(_vocabImageUrlDirectory)
                .Child(fileId);

            return Read(childQuery, fileId);
        }

        public IObservable<Unit> SaveVocabImageUrl(string fileId, string url)
        {
            ChildQuery childQuery = _downloadUrlsRef
                .Child(_vocabImageUrlDirectory)
                .Child(fileId);

            return Update(childQuery, url);
        }
    }
}