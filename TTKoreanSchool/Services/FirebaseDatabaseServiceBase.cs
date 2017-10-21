using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.Services
{
    public abstract class FirebaseDatabaseServiceBase<TDatabaseRef, TSnapshot> : IFirebaseDatabaseService
    {
        private readonly string _termsPathFormat = "terms/{0}";
        private readonly string _termTranslationsPathFormat = "terms/translations/{0}/{1}";
        private readonly string _sentencesPath = "sentences/examples";
        private readonly string _sentenceTranslationsPath = "sentences/translations";

        public FirebaseDatabaseServiceBase()
        {
            SentencesRef = GetRef(_sentencesPath);
            SentenceTranslationsRef = GetRef(_sentenceTranslationsPath);
        }

        protected TDatabaseRef TermsRef { get; private set; }

        protected TDatabaseRef TermTranslationsRef { get; private set; }

        protected TDatabaseRef SentencesRef { get; }

        protected TDatabaseRef SentenceTranslationsRef { get; }

        public IObservable<IReadOnlyList<Term>> GetTerms(string studySetId)
        {
            SetTermsRef(studySetId);
            SetTermTranslationsRef(studySetId, "en");

            return MultiQuery(TermsRef, TermTranslationsRef)
                .Select(snapHash => ConstructTerms(snapHash));
        }

        public IObservable<IReadOnlyList<Term>> GetSentences()
        {
            return MultiQuery(SentencesRef, SentenceTranslationsRef)
                .Select(snapHash => ConstructSentences(snapHash));
        }

        protected abstract TDatabaseRef GetRef(string path);

        protected abstract IObservable<Dictionary<string, TSnapshot>> MultiQuery(params TDatabaseRef[] dbRefs);

        protected abstract IReadOnlyList<Term> ConstructTerms(Dictionary<string, TSnapshot> snapHash);

        protected abstract IReadOnlyList<Term> ConstructSentences(Dictionary<string, TSnapshot> snapHash);

        private void SetTermsRef(string studySetId)
        {
            string path = string.Format(_termsPathFormat, studySetId);
            TermsRef = GetRef(path);
        }

        private void SetTermTranslationsRef(string studySetId, string lang)
        {
            string path = string.Format(_termTranslationsPathFormat, studySetId, lang);
            TermTranslationsRef = GetRef(path);
        }
    }
}