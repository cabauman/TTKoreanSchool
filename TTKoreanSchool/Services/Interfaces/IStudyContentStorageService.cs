using System;
using System.Collections.Generic;
using System.Reactive;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IStudyContentStorageService
    {
        bool FileExists(string filename);

        IObservable<string> GetVocabImageDownloadUrl(string filename);

        IObservable<KeyValuePair<string, string>> GetVocabImageDownloadUrls(params string[] filenames);

        IObservable<IDictionary<string, string>> GetVocabAudioDownloadUrls(params string[] filenames);

        IObservable<KeyValuePair<string, string>> GetSentenceAudioDownloadUrls(params string[] filenames);

        IObservable<Unit> SaveFileToLocalStorage(string directory, string filename, string downloadUrl);

        IObservable<Unit> SaveSentenceAudioFileToLocalStorage(string filename, string downloadUrl);

        IObservable<string> GetFileDownloadUrl(string directory, string filename);
    }
}