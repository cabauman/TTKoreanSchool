using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseStarredTermsRepo : FirebaseRepo<IDictionary<string, bool>>, IStarredTermsRepo
    {
        private readonly ChildQuery _starredTermsRef;

        public FirebaseStarredTermsRepo(FirebaseClient client)
        {
            _starredTermsRef = client.Child("starred-terms");
        }

        public IObservable<IDictionary<string, bool>> ReadAll(string uid)
        {
            ChildQuery childQuery = _starredTermsRef
                .Child(uid);

            return ReadAll(childQuery);
        }

        public IObservable<IDictionary<string, bool>> Read(string uid, string studySetId)
        {
            ChildQuery childQuery = _starredTermsRef
                .Child(uid)
                .Child(studySetId);

            return Read(childQuery, studySetId);
        }
    }
}