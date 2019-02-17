using System;
using System.Collections.ObjectModel;
using DynamicData;
using Splat;
using TTKSCore.Models;

namespace TongTongAdmin.Services
{
    public class StudyContentService
    {
        private SentenceRepo _sentenceRepo;
        private GrammarRepo _grammarRepo;
        private HomonymRepo _homonymRepo;
        private VocabTermRepo _vocabTermRepo;

        public StudyContentService(
            SentenceRepo sentenceRepo = null,
            GrammarRepo grammarRepo = null,
            HomonymRepo homonymRepo = null,
            VocabTermRepo vocabTermRepo = null)
        {
        }

        public IObservable<ReadOnlyObservableCollection<ExampleSentence>> GetSentenceChangeSet()
        {
            if (_sentenceRepo == null)
            {
                _sentenceRepo = Locator.Current.GetService<SentenceRepo>();
            }

            return _sentenceRepo.GetItems();
        }

        public IObservable<ReadOnlyObservableCollection<GrammarPrinciple>> GetGrammarChangeSet()
        {
            if (_grammarRepo == null)
            {
                _grammarRepo = Locator.Current.GetService<GrammarRepo>();
            }

            return _grammarRepo.GetItems();
        }

        public IObservable<ReadOnlyObservableCollection<StringEntity>> GetHomonymChangeSet()
        {
            if (_homonymRepo == null)
            {
                _homonymRepo = Locator.Current.GetService<HomonymRepo>();
            }

            return _homonymRepo.GetItems();
        }

        public IObservable<ReadOnlyObservableCollection<VocabTerm>> GetVocabChangeSet()
        {
            if (_vocabTermRepo == null)
            {
                _vocabTermRepo = Locator.Current.GetService<VocabTermRepo>();
            }

            return _vocabTermRepo.GetItems();
        }

        //public IObservable<IChangeSet<VocabTerm, string>> GetVocabChangeSet()
        //{
        //    if (_vocabTermRepo == null)
        //    {
        //        _vocabTermRepo = Locator.Current.GetService<VocabTermRepo>();
        //    }

        //    return _vocabTermRepo.GetChangeSet();
        //}

        public IObservable<IChangeSet<VocabImage, string>> VocabImageChangeSet()
        {
            throw new NotImplementedException();
        }
    }
}
