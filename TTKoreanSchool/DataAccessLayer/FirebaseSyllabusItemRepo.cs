using System;
using System.Collections.Generic;
using System.Reactive;
using Firebase.Database;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseSyllabusItemRepo : FirebaseRepo<SyllabusItem>, ISyllabusItemRepo
    {
        private readonly ChildQuery _syllabusesRef;

        public FirebaseSyllabusItemRepo(FirebaseClient client)
        {
            _syllabusesRef = client.Child("syllabuses");
        }

        public IObservable<SyllabusItem> ReadAll(string courseId)
        {
            ChildQuery childQuery = _syllabusesRef
                .Child(courseId);

            return ReadAll(childQuery);
        }

        public IObservable<Unit> Add(SyllabusItem syllabusItem, string courseId)
        {
            ChildQuery childQuery = _syllabusesRef
                .Child(courseId);

            return Add(childQuery, syllabusItem);
        }

        public IObservable<Unit> Update(SyllabusItem syllabusItem, string courseId)
        {
            ChildQuery childQuery = _syllabusesRef
                .Child(courseId)
                .Child(syllabusItem.Id);

            return Update(childQuery, syllabusItem);
        }

        public IObservable<Unit> Delete(string courseId, string syllabusItemId)
        {
            ChildQuery childQuery = _syllabusesRef
                .Child(courseId)
                .Child(syllabusItemId);

            return Delete(childQuery);
        }

        public IObservable<SyllabusItem> Observe(string courseId)
        {
            ChildQuery childQuery = _syllabusesRef
                .Child(courseId);

            return Observe(childQuery);
        }
    }
}