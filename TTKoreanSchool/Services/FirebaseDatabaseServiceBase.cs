using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.Services
{
    public abstract class FirebaseDatabaseServiceBase<TDatabaseRef, TSnapshot> : IFirebaseDatabaseService, IEnableLogger
    {
        private readonly string _termsPathFormat = "tt-study-sets/{0}";
        private readonly string _termTranslationsPathFormat = "tt-study-set-translations/{0}/{1}";
        private readonly string _sentencesPath = "sentences/examples";
        private readonly string _sentenceTranslationsPathFormat = "sentences/translations/{0}";
        private readonly string _vocabSectionsPath = "tt-study-set-sections2";
        private readonly string _vocabSubsectionsPathFormat = "tt-study-set-subsections/{0}";
        private readonly string _vocabImageUrlsPath = "vocab-image-urls";
        private readonly string _vocabImageUrlPathFormat = "vocab-image-urls/{0}";

        // Temporary until we fetch the device language.
        private readonly string _langCode = "en";

        public FirebaseDatabaseServiceBase()
        {
            SentencesRef = GetRef(_sentencesPath);
            VocabSectionsRef = GetRef(_vocabSectionsPath);
        }

        protected TDatabaseRef TermsRef { get; private set; }

        protected TDatabaseRef TermTranslationsRef { get; private set; }

        protected TDatabaseRef VocabSubsectionRef { get; private set; }

        protected TDatabaseRef SentencesRef { get; }

        protected TDatabaseRef SentenceTranslationsRef { get; private set; }

        protected TDatabaseRef VocabSectionsRef { get; }

        public IObservable<IReadOnlyList<VocabSection>> LoadVocabSections()
        {
            return Query(VocabSectionsRef)
                .Select(snapshot => ConstructVocabSections(snapshot));
        }

        public IObservable<IReadOnlyList<VocabSectionChild>> LoadVocabSetsInSubsection(string subsectionId)
        {
            SetVocabSubsectionRef(subsectionId);

            return Query(VocabSubsectionRef)
                .Select(snapshot => ConstructVocabSetsInSubsection(snapshot));
        }

        public IObservable<IReadOnlyList<Term>> LoadTerms(string studySetId)
        {
            SetTermsRef(studySetId);
            SetTermTranslationsRef(studySetId, _langCode);

            return MultiQuery(TermsRef, TermTranslationsRef)
                .Select(snapMap => ConstructTerms(snapMap));
        }

        public IObservable<IReadOnlyList<ExampleSentence>> LoadSentences()
        {
            SetSentenceTranslationsRef(_langCode);

            return MultiQuery(SentencesRef, SentenceTranslationsRef)
                .Select(snapMap => ConstructSentences(snapMap));
        }

        public IObservable<IReadOnlyDictionary<string, string>> LoadVocabImageUrls()
        {
            return Query(GetVocabImageUrlsRef())
                .Select(snapshot => ConstructVocabImageUrlMap(snapshot));
        }

        public abstract void SaveVocabImageUrl(string imageId, string url);

        protected abstract TDatabaseRef GetRef(string path);

        protected abstract IObservable<TSnapshot> Query(TDatabaseRef dbRef);

        protected abstract IObservable<Dictionary<string, TSnapshot>> MultiQuery(params TDatabaseRef[] dbRefs);

        protected abstract IReadOnlyList<VocabSection> ConstructVocabSections(TSnapshot snapshot);

        protected abstract IReadOnlyList<VocabSectionChild> ConstructVocabSetsInSubsection(TSnapshot snapshot);

        protected abstract IReadOnlyList<Term> ConstructTerms(Dictionary<string, TSnapshot> snapMap);

        protected abstract IReadOnlyList<ExampleSentence> ConstructSentences(Dictionary<string, TSnapshot> snapMap);

        protected abstract IReadOnlyDictionary<string, string> ConstructVocabImageUrlMap(TSnapshot snapshot);

        protected TDatabaseRef GetVocabImageUrlRef(string imageId)
        {
            string path = string.Format(_vocabImageUrlPathFormat, imageId);

            return GetRef(path);
        }

        protected TDatabaseRef GetVocabImageUrlsRef()
        {
            return GetRef(_vocabImageUrlsPath);
        }

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

        private void SetVocabSubsectionRef(string subsectionId)
        {
            string path = string.Format(_vocabSubsectionsPathFormat, subsectionId);
            VocabSubsectionRef = GetRef(path);
        }

        private void SetSentenceTranslationsRef(string lang)
        {
            string path = string.Format(_sentenceTranslationsPathFormat, lang);
            SentenceTranslationsRef = GetRef(path);
        }
    }
}