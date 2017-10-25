using System;
using System.Collections.Generic;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabSubsectionViewModel : IScreenViewModel
    {
        IReadOnlyList<VocabSectionChild> VocabSets { get; }

        void ItemSelected(VocabSectionChild selectedItem);
    }

    public class VocabSubsectionViewModel : BaseScreenViewModel, IVocabSubsectionViewModel
    {
        private IReadOnlyList<VocabSectionChild> _vocabSets;

        public VocabSubsectionViewModel(string subsectionId)
        {
            var database = Locator.Current.GetService<IFirebaseDatabaseService>();
            database.LoadVocabSetsInSubsection(subsectionId)
                .Subscribe(
                    vocabSets =>
                    {
                        VocabSets = vocabSets;
                    },
                    error =>
                    {
                        this.Log().Error(error.Message);
                    });
        }

        public IReadOnlyList<VocabSectionChild> VocabSets
        {
            get { return _vocabSets; }
            set { this.RaiseAndSetIfChanged(ref _vocabSets, value); }
        }

        public void ItemSelected(VocabSectionChild selectedItem)
        {
            var navService = Locator.Current.GetService<INavigationService>();
            var dialogService = Locator.Current.GetService<IDialogService>();
            var options = new AlertAction[]
            {
                new AlertAction(
                    "Mini Flashcards",
                    () =>
                    {
                        navService.PushScreen(new MiniFlashcardSetViewModel(selectedItem.Id));
                    }),

                new AlertAction(
                    "Detailed Flashcards",
                    () =>
                    {
                        navService.PushScreen(new DetailedFlashcardSetViewModel(selectedItem.Id));
                    })
            };

            dialogService.DisplayActionSheet("Study Activity", null, options);
        }
    }
}