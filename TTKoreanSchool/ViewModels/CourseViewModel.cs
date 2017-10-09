using ReactiveUI;

namespace TTKoreanSchool.ViewModels
{
    public class CourseViewModel : BaseViewModel
    {
        private string _title;
        private string _tuition;
        private string _thisSemester;

        public CourseViewModel()
        {
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
