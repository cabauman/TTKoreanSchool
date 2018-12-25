using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using TTKSCore.Models;

namespace TongTongAdmin.Services
{
    public class StudyContentService
    {
        public StudyContentService()
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

        public List<StringEntity> Homonyms { get; } = new List<StringEntity>
        {
            new StringEntity() { Id = "id1", Value = "123"},
            new StringEntity() { Id = "id2", Value = "456"},
            new StringEntity() { Id = "id3", Value = "789"},
        };
    }
}
