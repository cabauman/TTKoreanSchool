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

        protected override IReadOnlyList<VocabSectionChild> ConstructVocabSubsection(DataSnapshot snapshot)
        {
            if(!snapshot.Exists)
            {
                return null;
            }

            var studySetList = new List<VocabSectionChild>((int)snapshot.ChildrenCount);
            NSEnumerator studySets = snapshot.Children;
            var studySetSnap = studySets.NextObject() as DataSnapshot;

            while(studySetSnap != null)
            {
                var data = studySetSnap.GetValue<NSDictionary>();

                string id = studySetSnap.Key;
                string title = data["title"].ToString();
                string iconId = data["iconId"].ToString();

                var studySet = new VocabSectionChild(id, title, iconId, false);
                studySetList.Add(studySet);
                studySetSnap = studySets.NextObject() as DataSnapshot;
            }

            return studySetList.AsReadOnly();
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

            var termsSnap = snapHash[TermsRef.Url];
            var translationsSnap = snapHash[TermTranslationsRef.Url];

            var termsData = termsSnap.GetValue<NSDictionary>();
            var translationsData = translationsSnap.GetValue<NSDictionary>();

            string username = termsData["username"].ToString();
            bool isAdmin = ((NSNumber)termsData["isAdmin"]).BoolValue;
            string email = translationsData["translation"].ToString();

            var term = new Term();

            return new List<Term> { term }.AsReadOnly();
        }

        protected override IReadOnlyList<ExampleSentence> ConstructSentences(Dictionary<string, DataSnapshot> snapHash)
        {
            throw new NotImplementedException();
        }
    }
}