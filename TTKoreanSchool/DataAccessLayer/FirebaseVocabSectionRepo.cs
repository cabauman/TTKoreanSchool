using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseVocabSectionRepo : FirebaseRepo<VocabSection>, IVocabSectionRepo
    {
        private readonly ChildQuery _sectionsRef;

        public FirebaseVocabSectionRepo(FirebaseClient client)
        {
            _sectionsRef = client.Child("tt-study-set-sections2");
        }

        public IObservable<VocabSection> ReadAll()
        {
            return ReadAll(_sectionsRef);
        }
    }
}