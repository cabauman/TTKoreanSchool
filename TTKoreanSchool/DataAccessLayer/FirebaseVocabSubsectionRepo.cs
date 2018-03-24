using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseVocabSubsectionRepo : FirebaseRepo<IDictionary<string, string>>, IVocabSubsectionRepo
    {
        private readonly ChildQuery _subsectionsRef;

        public FirebaseVocabSubsectionRepo(FirebaseClient client)
        {
            _subsectionsRef = client.Child("tt-study-set-subsections");
        }

        public IObservable<IDictionary<string, string>> ReadSubsection(string subsectionId)
        {
            ChildQuery childQuery = _subsectionsRef
                .Child(subsectionId);

            return Read(childQuery, subsectionId);
        }
    }
}