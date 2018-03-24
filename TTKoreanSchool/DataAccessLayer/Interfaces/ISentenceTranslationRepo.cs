using System;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface ISentenceTranslationRepo
    {
        IObservable<string> ReadAll(string langCode);

        IObservable<string> Read(string langCode, string sentenceId);
    }
}