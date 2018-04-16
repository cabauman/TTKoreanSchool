using System;
using System.Collections.Generic;
using System.Reactive;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IStudyContentDownloadUrlRepo
    {
        IObservable<string> ReadAll(string uid);

        IObservable<string> ReadVocabImageUrl(string fileId);

        IObservable<Unit> SaveVocabImageUrl(string fileId, string url);
    }
}