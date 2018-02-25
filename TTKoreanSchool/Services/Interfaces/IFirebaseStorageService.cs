using System;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IFirebaseStorageService
    {
        IObservable<string> GetDownloadUrlForVocabImage(string imageId);

        IObservable<string> DownloadVocabAudio(string filename, string localUrl = null);

        IObservable<string> DownloadSentenceAudio(string filename, string localUrl);
    }
}