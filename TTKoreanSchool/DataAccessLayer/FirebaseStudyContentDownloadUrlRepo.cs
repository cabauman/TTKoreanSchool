using System;
using System.Collections.Generic;
using System.Reactive;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseStudyContentDownloadUrlRepo : FirebaseRepo<string>, IStudyContentDownloadUrlRepo
    {
        public const string VOCAB_IMAGE_URL_DIRECTORY = "term-images";

        private readonly ChildQuery _downloadUrlsRef;

        private RealtimeDatabase<string> _vocabImageUrlDb;
        private RealtimeDatabase<string> _vocabAudioUrlDb;
        private RealtimeDatabase<string> _sentenceAudioUrlDb;

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

        public IObservable<string> Read(string directory, string filename)
        {
            RealtimeDatabase<string> db = null;
            switch(directory)
            {
                case VOCAB_IMAGE_URL_DIRECTORY:
                    db = _vocabImageUrlDb;
                    break;
            }
            
            if(db == null)
            {
                ChildQuery childQuery = _downloadUrlsRef
                    .Child(directory);

                db = ReadAll(childQuery);
            }

            return ReadAll(childQuery);
        }

        public IObservable<string> ReadVocabImageUrl(string fileId)
        {
            ChildQuery childQuery = _downloadUrlsRef
                .Child(VocabImageUrlDirectory)
                .Child(fileId);

            return Read(childQuery, fileId);
        }

        public IObservable<Unit> SaveVocabImageUrl(string fileId, string url)
        {
            ChildQuery childQuery = _downloadUrlsRef
                .Child(VocabImageUrlDirectory)
                .Child(fileId);

            return Update(childQuery, url);
        }
    }
}