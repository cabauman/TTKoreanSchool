using System;
using System.Collections.Generic;
using Firebase.Database;
using Foundation;
using GameCtor.Firebase.Database.Rx;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services;

namespace TTKoreanSchool.iOS.Services
{
    public class FirebaseDatabaseService : FirebaseDatabaseServiceBase<DatabaseReference, DataSnapshot>
    {
        private readonly DatabaseReference _root = Database.DefaultInstance.GetRootReference();

        protected override DatabaseReference GetRef(string path)
        {
            var dbRef = _root.GetChild(path);

            return dbRef;
        }

        protected override IObservable<DataSnapshot> Query(DatabaseReference dbRef)
        {
            return dbRef.ObserveSingleEventRx(DataEventType.Value);
        }

        protected override IObservable<Dictionary<string, DataSnapshot>> MultiQuery(params DatabaseReference[] dbRefs)
        {
            return FirebaseDatabaseUtils.ObserveSingleZippedEvent(dbRefs);
        }

        protected override IReadOnlyList<VocabSection> ConstructVocabSections(DataSnapshot snapshot)
        {
            if(!snapshot.Exists)
            {
                return null;
            }

            var sectionList = new List<VocabSection>((int)snapshot.ChildrenCount);
            NSEnumerator sections = snapshot.Children;
            var sectionSnap = sections.NextObject() as DataSnapshot;

            while(sectionSnap != null)
            {
                var data = sectionSnap.GetValue<NSDictionary>();

                string id = sectionSnap.Key;
                string title = data["title"].ToString();
                string colorTheme = data["colorTheme"].ToString();

                var sectionChildren = GetSectionChildren(sectionSnap);
                var section = new VocabSection(id, title, colorTheme, sectionChildren);
                sectionList.Add(section);
                sectionSnap = sections.NextObject() as DataSnapshot;
            }

            return sectionList.AsReadOnly();
        }

        private IReadOnlyList<VocabSectionChild> GetSectionChildren(DataSnapshot sectionSnap)
        {
            // Currently only supports sections that contain all subsections OR all sets, not both
            bool childrenAreSubsections = false;
            var sectionChildrenSnap = sectionSnap.GetChildSnapshot("study-sets");
            if(!sectionChildrenSnap.Exists)
            {
                childrenAreSubsections = true;
                sectionChildrenSnap = sectionSnap.GetChildSnapshot("subsections");
            }

            NSEnumerator sectionChildren = sectionChildrenSnap.Children;
            var sectionChildSnap = sectionChildren.NextObject() as DataSnapshot;
            var children = new List<VocabSectionChild>();
            while(sectionChildSnap != null)
            {
                var data = sectionChildSnap.GetValue<NSDictionary>();

                string id = sectionChildSnap.Key;
                string title = data["title"].ToString();
                string iconId = data["iconId"].ToString();
                bool isSubsection = childrenAreSubsections;

                var sectionChild = new VocabSectionChild(id, title, iconId, isSubsection);

                children.Add(sectionChild);
                sectionChildSnap = sectionChildren.NextObject() as DataSnapshot;
            }

            return children.AsReadOnly();
        }

        protected override IReadOnlyList<VocabSectionChild> ConstructVocabSetsInSubsection(DataSnapshot snapshot)
        {
            if(!snapshot.Exists)
            {
                return null;
            }

            var vocabSets = new List<VocabSectionChild>((int)snapshot.ChildrenCount);
            NSEnumerator studySets = snapshot.Children;
            var studySetSnap = studySets.NextObject() as DataSnapshot;

            while(studySetSnap != null)
            {
                string id = studySetSnap.Key;
                string title = studySetSnap.GetValue<NSString>();

                var studySet = new VocabSectionChild(id, title, null, false);
                vocabSets.Add(studySet);
                studySetSnap = studySets.NextObject() as DataSnapshot;
            }

            return vocabSets.AsReadOnly();
        }

        protected override IReadOnlyList<Term> ConstructTerms(Dictionary<string, DataSnapshot> snapHash)
        {
            foreach(var snap in snapHash.Values)
            {
                if(!snap.Exists)
                {
                    return null;
                }
            }

            var termListSnap = snapHash[TermsRef.Url];
            var translationListSnap = snapHash[TermTranslationsRef.Url];

            var terms = new List<Term>((int)termListSnap.ChildrenCount);

            NSEnumerator termsChildren = termListSnap.Children;
            NSEnumerator translationsChildren = translationListSnap.Children;

            var termSnap = termsChildren.NextObject() as DataSnapshot;
            var translationSnap = translationsChildren.NextObject() as DataSnapshot;

            while(termSnap != null)
            {
                var termData = termSnap.GetValue<NSDictionary>();

                var id = termSnap.Key;
                var ko = termData["ko"].ToString();
                var romanization = termData["romanization"].ToString();
                var imageIds = termData["imageIds"]?.ToString().Split(',');
                var sentenceIds = termData["sentenceIds"]?.ToString().Split(',');
                var translation = translationSnap.GetValue<NSString>();

                var item = new Term(id, ko, romanization, translation, null, imageIds, sentenceIds);
                terms.Add(item);

                termSnap = termsChildren.NextObject() as DataSnapshot;
                translationSnap = translationsChildren.NextObject() as DataSnapshot;
            }

            return terms.AsReadOnly();
        }

        protected override IReadOnlyList<ExampleSentence> ConstructSentences(Dictionary<string, DataSnapshot> snapHash)
        {
            throw new NotImplementedException();
        }
    }
}