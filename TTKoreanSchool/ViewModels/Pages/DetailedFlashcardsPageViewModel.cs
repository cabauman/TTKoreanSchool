extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IDetailedFlashcardsPageViewModel : IPageViewModel
    {
        IReadOnlyList<Term> Terms { get; }

        IReadOnlyList<ExampleSentence> Sentences { get; }

        IDetailedFlashcardViewModel GetFlashcardAtIndex(int index);
    }

    public class DetailedFlashcardsPageViewModel : BasePageViewModel, IDetailedFlashcardsPageViewModel
    {
        public DetailedFlashcardsPageViewModel(string vocabSetId)
        {
            var database = Locator.Current.GetService<IStudyContentDataService>();

            //database.LoadTerms(vocabSetId)
            //    .Do(terms => Terms = terms)
            //    .SelectMany(terms => terms.AsEnumerable())
            //    .Select(
            //        term =>
            //        {
            //            if(term.AudioVersion > 0)
            //            {
            //                var storage = Locator.Current.GetService<IStorageService>();
            //                return storage.DownloadVocabAudio(term.Romanization + ".mp3");
            //            }
            //            else
            //            {
            //                return Observable.Empty<string>();
            //            }
            //        })
            //    .Merge()
            //    .Subscribe(
            //        _ =>
            //        {
            //            Console.WriteLine(_);
            //        },
            //        error =>
            //        {
            //            this.Log().Error(error.Message);
            //        });

            this.Log().Debug("All done!");

            //database.LoadSentences()
            //    .Subscribe(
            //        sentences =>
            //        {
            //            Sentences = sentences;
            //        },
            //        error =>
            //        {
            //            this.Log().Error(error.Message);
            //        });
        }

        public IReadOnlyList<Term> Terms { get; private set; }

        public IReadOnlyList<ExampleSentence> Sentences { get; private set; }

        public IDetailedFlashcardViewModel GetFlashcardAtIndex(int index)
        {
            var term = Terms[index];
            var sentencesUsingTerm = new List<ExampleSentence>();
            foreach(var id in term.SentenceIds)
            {
                sentencesUsingTerm.Add(Sentences[int.Parse(id)]);
            }

            var flashcard = new DetailedFlashcardViewModel(term, sentencesUsingTerm.AsReadOnly());

            return flashcard;
        }
    }
}