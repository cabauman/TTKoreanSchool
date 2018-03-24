using System;
using System.Collections.Generic;
using System.Reactive;
using TTKoreanSchool.Models;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IStudyContentDataService
    {
        IObservable<IList<IVocabSectionViewModel>> GetVocabSections();

        IObservable<IList<IMiniFlashcardViewModel>> GetMiniFlashcards(string studySetId);

        IObservable<IList<IDetailedFlashcardViewModel>> GetDetailedFlashcards(string studySetId);

        IObservable<string> GetVocabImageUrl(string imageId);

        IObservable<Unit> SaveVocabImageUrl(string imageId, string url);

        //IObservable<IList<Term>> GetGrammarTerms(string studySetId);

        IObservable<IList<IExampleSentenceViewModel>> GetSentences();
    }
}