using System;
using Firebase.Database;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseCourseRepo<T> : FirebaseRepo<Course>, ICourseRepo
    {
        private readonly ChildQuery _coursesRef;

        public FirebaseCourseRepo(FirebaseClient client)
        {
            _coursesRef = client.Child("courses");
        }

        public IObservable<Course> ReadAll()
        {
            return ReadAll(_coursesRef);
        }

        public IObservable<Course> Read(string courseId)
        {
            ChildQuery childQuery = _coursesRef
                .Child(courseId);

            return Read(childQuery, courseId);
        }
    }
}