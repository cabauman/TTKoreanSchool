using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IDetailedFlashcardSetViewModel : IScreenViewModel
    {
        IReadOnlyList<Term> Terms { get; }

        IReadOnlyList<ExampleSentence> Sentences { get; }

        IDetailedFlashcardViewModel GetFlashcardAtIndex(int index);
    }

    public class DetailedFlashcardSetViewModel : BaseScreenViewModel, IDetailedFlashcardSetViewModel
    {
        public DetailedFlashcardSetViewModel(string vocabSetId)
        {
            var database = Locator.Current.GetService<IFirebaseDatabaseService>();

            database.LoadTerms(vocabSetId)
                .Do(terms => Terms = terms)
                .SelectMany(terms => terms.AsEnumerable())
                .Select(
                    term =>
                    {
                        if(term.AudioVersion > 0)
                        {
                            var storage = Locator.Current.GetService<IFirebaseStorageService>();
                            return storage.DownloadVocabAudio(term.Romanization + ".mp3");
                        }
                        else
                        {
                            return Observable.Empty<string>();
                        }
                    })
                .Merge()
                .Subscribe(
                    _ =>
                    {
                        Console.WriteLine(_);
                    },
                    error =>
                    {
                        this.Log().Error(error.Message);
                    });

            this.Log().Debug("All done!");

            database.LoadSentences()
                .Subscribe(
                    sentences =>
                    {
                        Sentences = sentences;
                    },
                    error =>
                    {
                        this.Log().Error(error.Message);
                    });
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