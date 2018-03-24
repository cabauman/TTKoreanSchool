using ReactiveUI;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.ViewModels
{
    public class CourseViewModel : BaseViewModel
    {
        private Course _course;
        private string _title;
        private string _tuition;
        private string _thisSemester;

        public CourseViewModel()
        {
        }

        public CourseViewModel(Course course)
        {
            _course = course;
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        public string Tuition
        {
            get { return _tuition; }
            set { this.RaiseAndSetIfChanged(ref _tuition, value); }
        }

        public string ThisSemester
        {
            get { return _thisSemester; }
            set { this.RaiseAndSetIfChanged(ref _thisSemester, value); }
        }
    }
}