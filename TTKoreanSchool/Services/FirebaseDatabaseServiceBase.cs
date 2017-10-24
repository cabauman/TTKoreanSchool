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
        private readonly string _termTranslationsPathFormat = "tt-study-sets-translations/{0}/{1}";
        private readonly string _sentencesPath = "sentences/examples";
        private readonly string _sentenceTranslationsPath = "sentences/translations";
        private readonly string _vocabSectionsPath = "tt-study-set-sections2";
        private readonly string _vocabSubsectionsPathFormat = "tt-study-set-subsections/{0}";

        public FirebaseDatabaseServiceBase()
        {
            SentencesRef = GetRef(_sentencesPath);
            SentenceTranslationsRef = GetRef(_sentenceTranslationsPath);
            VocabSectionsRef = GetRef(_vocabSectionsPath);
        }

        protected TDatabaseRef TermsRef { get; private set; }

        protected TDatabaseRef TermTranslationsRef { get; private set; }

        protected TDatabaseRef VocabSubsectionRef { get; private set; }

        protected TDatabaseRef SentencesRef { get; }

        protected TDatabaseRef SentenceTranslationsRef { get; }

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
                .Select(snapshot => ConstructVocabSubsection(snapshot));
        }

        public IObservable<IReadOnlyList<Term>> LoadTerms(string studySetId)
        {
            SetTermsRef(studySetId);
            SetTermTranslationsRef(studySetId, "en");

            return MultiQuery(TermsRef, TermTranslationsRef)
                .Select(snapHash => ConstructTerms(snapHash));
        }

        public IObservable<IReadOnlyList<ExampleSentence>> LoadSentences()
        {
            return MultiQuery(SentencesRef, SentenceTranslationsRef)
                .Select(snapHash => ConstructSentences(snapHash));
        }

        protected abstract TDatabaseRef GetRef(string path);

        protected abstract IObservable<TSnapshot> Query(TDatabaseRef dbRef);

        protected abstract IObservable<Dictionary<string, TSnapshot>> MultiQuery(params TDatabaseRef[] dbRefs);

        protected abstract IReadOnlyList<VocabSection> ConstructVocabSections(TSnapshot snapshot);

        protected abstract IReadOnlyList<VocabSectionChild> ConstructVocabSubsection(TSnapshot snapshot);

        protected abstract IReadOnlyList<Term> ConstructTerms(Dictionary<string, TSnapshot> snapHash);

        protected abstract IReadOnlyList<ExampleSentence> ConstructSentences(Dictionary<string, TSnapshot> snapHash);

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
    }
}