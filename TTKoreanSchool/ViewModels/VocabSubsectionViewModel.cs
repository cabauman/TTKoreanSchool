using System;
using System.Collections.Generic;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabSubsectionViewModel : IScreenViewModel
    {
    }

    public class VocabSubsectionViewModel : BaseScreenViewModel, IVocabSubsectionViewModel
    {
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

        public IReadOnlyList<VocabSectionChild> VocabSets { get; private set; }

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