using System;
using DynamicData;
using Splat;
using TTKSCore.Models;

namespace TongTongAdmin.Services
{
    public class StudyContentService
    {
        private HomonymRepo _homonymRepo;
        private ISourceCache<StringEntity, string> _homonymCache;

        public StudyContentService(HomonymRepo homonymRepo = null)
        {
            //VocabCache = new SourceCache<VocabTerm, string>(x => x.Id);
            //GrammarCache = new SourceCache<GrammarPrinciple, string>(x => x.Id);
            //SentenceCache = new SourceCache<ExampleSentence, string>(x => x.Id);
            //VocabImageCache = new SourceCache<VocabImage, string>(x => x.Id);
        }

        public ISourceCache<VocabTerm, string> VocabCache { get; }

        public ISourceCache<GrammarPrinciple, string> GrammarCache { get; }

        public ISourceCache<ExampleSentence, string> SentenceCache { get; }

        public ISourceCache<VocabImage, string> VocabImageCache { get; }

        public IObservable<IChangeSet<StringEntity, string>> GetHomonymChangeSet()
        {
            if (_homonymRepo == null)
            {
                _homonymRepo = Locator.Current.GetService<HomonymRepo>();
            }

            return _homonymRepo.GetChangeSet();
        }
    }
}
