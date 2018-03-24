using System;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IVocabTermRepo
    {
        IObservable<Term> ReadStudySet(string langCode, string studySetId);
    }
}