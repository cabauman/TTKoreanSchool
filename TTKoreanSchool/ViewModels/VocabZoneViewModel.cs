using System;
using System.Collections.Generic;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabZoneViewModel : IScreenViewModel
    {
        IReadOnlyList<VocabSection> Sections { get; }

        void ItemSelected(VocabSectionChild selectedItem);
    }

    public class VocabZoneViewModel : BaseScreenViewModel, IVocabZoneViewModel
    {
        private IReadOnlyList<VocabSection> _sections;

        public VocabZoneViewModel()
        {
            _sections = new List<VocabSection>();

            var database = Locator.Current.GetService<IFirebaseDatabaseService>();
            database.LoadVocabSections()
                .Subscribe(
                    sections =>
                    {
                        Sections = sections;
                    },
                    error =>
                    {
                        this.Log().Error(error.Message);
                    });
        }

        public IReadOnlyList<VocabSection> Sections
        {
            get { return _sections; }
            set { this.RaiseAndSetIfChanged(ref _sections, value); }
        }

        public void ItemSelected(VocabSectionChild selectedItem)
        {
            if(selectedItem.IsSubsection)
            {
                var navService = Locator.Current.GetService<INavigationService>();
                navService.PushScreen(new VocabSubsectionViewModel(selectedItem.Id));
            }
            else
            {
                var dialogService = Locator.Current.GetService<IDialogService>();
                var options = new AlertAction[]
                {
                    new AlertAction(
                        "Mini Flashcards",
                        () =>
                        {
                            var navService = Locator.Current.GetService<INavigationService>();
                            navService.PushScreen(new MiniFlashcardSetViewModel(selectedItem.Id));
                        }),

                    new AlertAction(
                        "Detailed Flashcards",
                        () =>
                        {
                            var navService = Locator.Current.GetService<INavigationService>();
                            navService.PushScreen(new DetailedFlashcardSetViewModel(selectedItem.Id));
                        })
                };

                dialogService.DisplayActionSheet("Study Activity", null, options);
            }
        }
    }
}