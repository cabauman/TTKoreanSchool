using System;
using System.Collections.Generic;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IStudyContentStorageService
    {
        IObservable<string> GetVocabImageDownloadUrls(params string[] filenames);

        IObservable<IDictionary<string, string>> GetVocabAudioDownloadUrls(params string[] filenames);

        IObservable<IDictionary<string, string>> GetSentenceAudioDownloadUrls(params string[] filenames);
    }
}