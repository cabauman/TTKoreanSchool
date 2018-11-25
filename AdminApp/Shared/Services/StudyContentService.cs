using System;
using System.Collections.Generic;
using DynamicData;
using TTKSCore.Models;

namespace TongTongAdmin.Services
{
    public class StudyContentService
    {
        public StudyContentService()
        {
            VocabCache = new SourceCache<VocabTerm, string>(x => x.Id);
            GrammarCache = new SourceCache<GrammarPrinciple, string>(x => x.Id);
            SentenceCache = new SourceCache<ExampleSentence, string>(x => x.Id);
            VocabImageCache = new SourceCache<VocabImage, string>(x => x.Id);
        }

        public ISourceCache<VocabTerm, string> VocabCache { get; }

        public ISourceCache<GrammarPrinciple, string> GrammarCache { get; }

        public ISourceCache<ExampleSentence, string> SentenceCache { get; }

        public ISourceCache<VocabImage, string> VocabImageCache { get; }
    }
}
