using System;
using System.Collections.Generic;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IMiniFlashcardSetViewModel : IScreenViewModel
    {
    }

    public class MiniFlashcardSetViewModel : BaseScreenViewModel, IMiniFlashcardSetViewModel
    {
        public MiniFlashcardSetViewModel(string vocabSetId)
        {
            var database = Locator.Current.GetService<IFirebaseDatabaseService>();
            database.LoadTerms(vocabSetId)
                .Subscribe(
                    terms =>
                    {
                        Terms = terms;
                    },
                    error =>
                    {
                        this.Log().Error(error.Message);
                    });
        }

        public IReadOnlyList<Term> Terms { get; private set; }
    }
}