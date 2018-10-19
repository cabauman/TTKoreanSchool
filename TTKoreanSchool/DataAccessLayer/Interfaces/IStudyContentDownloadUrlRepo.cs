using System;
using System.Collections.Generic;
using System.Reactive;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IStudyContentDownloadUrlRepo
    {
        IObservable<string> ReadAll(string directory);

        IObservable<string> Read(string directory, string filename);

        IObservable<string> ReadVocabImageUrl(string fileId);

        IObservable<IDictionary<string, string>> ReadVocabImageUrls();

        IObservable<IDictionary<string, string>> ReadVocabAudioUrls();

        IObservable<IDictionary<string, string>> ReadSentenceAudioUrls();

        IObservable<Unit> SaveVocabImageUrl(string fileId, string url);

        IObservable<Unit> Save(string directory, string filename, string url);
    }
}