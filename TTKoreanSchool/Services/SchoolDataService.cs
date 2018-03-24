using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services
{
    public class SchoolDataService : ISchoolService
    {
        private readonly ICourseRepo _courseRepo;
        private readonly ISyllabusItemRepo _syllabusItemRepo;

        public SchoolDataService(ICourseRepo courseRepo, ISyllabusItemRepo syllabusItemRepo)
        {
            _courseRepo = courseRepo;
            _syllabusItemRepo = syllabusItemRepo;
        }

        public IObservable<CourseViewModel> GetCourse(string courseId)
        {
            return _courseRepo
                .Read(courseId)
                .Select(model => new CourseViewModel(model));
        }

        public IObservable<IList<SyllabusItemViewModel>> GetSyllabusItems(string courseId)
        {
            return _syllabusItemRepo
                .ReadAll(courseId)
                .Select(model => new SyllabusItemViewModel(model))
                .ToList();
        }
    }
}