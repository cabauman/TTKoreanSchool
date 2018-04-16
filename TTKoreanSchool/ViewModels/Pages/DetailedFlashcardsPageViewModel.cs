extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IDetailedFlashcardsPageViewModel : IPageViewModel
    {
        IList<Term> Terms { get; }

        IList<IExampleSentenceViewModel> Sentences { get; }

        ReactiveCommand<Unit, IList<IExampleSentenceViewModel>> LoadSentences { get; }

        IDetailedFlashcardViewModel GetFlashcardAtIndex(int index);
    }

    public class DetailedFlashcardsPageViewModel : BasePageViewModel, IDetailedFlashcardsPageViewModel
    {
        private readonly INavigationService _navService;
        private readonly IStudyContentDataService _dataService;
        private readonly ObservableAsPropertyHelper<IList<IExampleSentenceViewModel>> _sentences;

        public DetailedFlashcardsPageViewModel(
            INavigationService navService = null,
            IStudyContentDataService dataService = null)
        {
            _navService = navService ?? Locator.Current.GetService<INavigationService>();
            _dataService = dataService ?? Locator.Current.GetService<IStudyContentDataService>();

            LoadSentences = ReactiveCommand.CreateFromObservable(() => _dataService.GetSentences());
            LoadSentences.ToProperty(this, x => x.Sentences, out _sentences);
            LoadSentences.ThrownExceptions.Subscribe(
                ex =>
                {
                    throw new Exception(ex.ToString());
                });
        }

        public IList<Term> Terms { get; }

        public IList<IExampleSentenceViewModel> Sentences
        {
            get { return _sentences.Value; }
        }

        public ReactiveCommand<Unit, IList<IExampleSentenceViewModel>> LoadSentences { get; }

        public IDetailedFlashcardViewModel GetFlashcardAtIndex(int index)
        {
            var term = Terms[index];
            var sentencesUsingTerm = new List<IExampleSentenceViewModel>();
            string[] sentenceIds = term.SentenceIds.Split(',');
            foreach(var sentenceId in sentenceIds)
            {
                sentencesUsingTerm.Add(Sentences[int.Parse(sentenceId)]);
            }

            string[] imageIds = term.ImageIds.Split(',');
            foreach(var imageId in imageIds)
            {
                sentencesUsingTerm.Add(Sentences[int.Parse(imageId)]);
            }

            var flashcard = new DetailedFlashcardViewModel(term, sentencesUsingTerm.AsReadOnly());

            return flashcard;
        }
    }
}