using System;
using System.Collections.Generic;
using Firebase.Database;
using Foundation;
using GameCtor.Firebase.Database.Rx;
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

        protected override IObservable<Dictionary<string, DataSnapshot>> MultiQuery(params DatabaseReference[] dbRefs)
        {
            return FirebaseDatabaseUtils.ObserveSingleZippedEvent(dbRefs);
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

        protected override IReadOnlyList<Term> ConstructSentences(Dictionary<string, DataSnapshot> snapHash)
        {
            throw new NotImplementedException();
        }
    }
}