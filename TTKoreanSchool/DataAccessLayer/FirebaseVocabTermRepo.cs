using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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

        private string _currentStudySetId;
        private IEnumerable<Term> _currentStudySet;

        public FirebaseVocabTermRepo(FirebaseClient client)
        {
            _studySetsRef = client.Child("tt-study-sets");
            _translationsRef = client.Child("tt-study-set-translations");
        }

        public IObservable<Term> ReadStudySet(string langCode, string studySetId)
        {
            if(_currentStudySetId == studySetId && _currentStudySet != null)
            {
                return _currentStudySet.ToObservable();
            }

            _currentStudySetId = studySetId;

            ChildQuery termsQuery = _studySetsRef
                .Child(studySetId);

            ChildQuery translationsQuery = _translationsRef
                .Child(studySetId)
                .Child(langCode);

            var terms = ReadAll(termsQuery, studySetId);
            var translations = ReadAllBasicType<string>(translationsQuery);

            var termsObservable = Observable
                .Zip(
                    terms,
                    translations,
                    (term, translation) =>
                    {
                        term.Translation = translation;
                        return term;
                    });

            _currentStudySet = termsObservable.ToEnumerable();

            return termsObservable;
        }
    }
}