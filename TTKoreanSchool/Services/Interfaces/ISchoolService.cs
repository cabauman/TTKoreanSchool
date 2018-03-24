using System;
using System.Collections.Generic;
using System.Text;
using TTKoreanSchool.Models;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface ISchoolService
    {
        IObservable<CourseViewModel> GetCourse(string courseId);

        IObservable<IList<SyllabusItemViewModel>> GetSyllabusItems(string courseId);
    }
}