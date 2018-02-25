using System;
using System.Collections.Generic;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IFirebaseDatabaseService
    {
        IObservable<IReadOnlyList<VocabSection>> LoadVocabSections();

        IObservable<IReadOnlyList<VocabSectionChild>> LoadVocabSetsInSubsection(string subsectionId);

        IObservable<IReadOnlyList<Term>> LoadTerms(string studySetId);

        IObservable<IReadOnlyList<ExampleSentence>> LoadSentences();

        IObservable<IReadOnlyDictionary<string, string>> LoadVocabImageUrls();

        void SaveVocabImageUrl(string imageId, string url);
    }
}