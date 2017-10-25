using System;
using System.Collections.Generic;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IMiniFlashcardSetViewModel : IScreenViewModel
    {
        IReadOnlyList<Term> Terms { get; }
    }

    public class MiniFlashcardSetViewModel : BaseScreenViewModel, IMiniFlashcardSetViewModel
    {
        private IReadOnlyList<Term> _terms;

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

        public IReadOnlyList<Term> Terms
        {
            get { return _terms; }
            set { this.RaiseAndSetIfChanged(ref _terms, value); }
        }
    }
}