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
    public class FirebaseVocabTermRepo : FirebaseRepo<Term>, IVocabTermRepo
    {
        private readonly ChildQuery _studySetsRef;
        private readonly ChildQuery _translationsRef;

        public FirebaseVocabTermRepo(FirebaseClient client)
        {
            _studySetsRef = client.Child("tt-study-sets");
            _translationsRef = client.Child("tt-study-set-translations");
        }

        public IObservable<Term> ReadStudySet(string langCode, string studySetId)
        {
            ChildQuery translationsQuery = _translationsRef
                .Child(studySetId)
                .Child(langCode);

            var translations = ReadAllBasicType<string>(translationsQuery);

            ChildQuery termsQuery = _studySetsRef
                .Child(studySetId);

            return ReadAll(termsQuery)
                .Zip(
                    translations,
                    (term, translation) =>
                    {
                        term.Translation = translation;
                        return term;
                    });
        }
    }
}