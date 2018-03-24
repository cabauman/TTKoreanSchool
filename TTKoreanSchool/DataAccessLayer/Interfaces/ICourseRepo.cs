using System;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface ICourseRepo
    {
        //IObservable<IEnumerable<Course>> Find(Func<Course, bool> predicate);

        IObservable<Course> ReadAll();

        IObservable<Course> Read(string courseId);
    }
}