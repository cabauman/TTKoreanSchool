using System;
using System.Collections.ObjectModel;
using DynamicData;
using Splat;
using TTKS.Core.Models;

namespace TTKS.Admin.Services
{
    public class StudyContentService
    {
        private SentenceRepository _sentenceRepo;
        private GrammarPrincipleRepository _grammarRepo;
        private HomonymRepository _homonymRepo;
        private VocabTermRepository _vocabTermRepo;

        public StudyContentService(
            SentenceRepository sentenceRepo = null,
            GrammarPrincipleRepository grammarRepo = null,
            HomonymRepository homonymRepo = null,
            VocabTermRepository vocabTermRepo = null)
        {
        }

        public IObservable<ReadOnlyObservableCollection<ExampleSentence>> GetSentenceChangeSet()
        {
            if (_sentenceRepo == null)
            {
                _sentenceRepo = Locator.Current.GetService<SentenceRepository>();
            }

            return _sentenceRepo.GetItems();
        }

        public IObservable<ReadOnlyObservableCollection<GrammarPrinciple>> GetGrammarChangeSet()
        {
            if (_grammarRepo == null)
            {
                _grammarRepo = Locator.Current.GetService<GrammarPrincipleRepository>();
            }

            return _grammarRepo.GetItems();
        }

        public IObservable<ReadOnlyObservableCollection<StringEntity>> GetHomonymChangeSet()
        {
            if (_homonymRepo == null)
            {
                _homonymRepo = Locator.Current.GetService<HomonymRepository>();
            }

            return _homonymRepo.GetItems();
        }

        public IObservable<ReadOnlyObservableCollection<VocabTerm>> GetVocabChangeSet()
        {
            if (_vocabTermRepo == null)
            {
                _vocabTermRepo = Locator.Current.GetService<VocabTermRepository>();
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
