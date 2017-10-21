using System;
using System.Collections.Generic;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IFirebaseDatabaseService
    {
        IObservable<IReadOnlyList<Term>> GetTerms(string studySetId);

        IObservable<IReadOnlyList<Term>> GetSentences();
    }
}