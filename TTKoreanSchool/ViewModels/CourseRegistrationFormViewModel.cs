using ReactiveUI;

namespace TTKoreanSchool.ViewModels
{
    public class CourseRegistrationFormViewModel : BaseScreenViewModel
    {
        private string _courseTitle;

        public CourseRegistrationFormViewModel()
        {
        }

        public ReactiveCommand SubmitRegistration { get; }

        public ReactiveCommand DiscardRegistration { get; }

        public string CourseTitle
        {
            get { return _courseTitle; }
            set { this.RaiseAndSetIfChanged(ref _courseTitle, value); }
        }
    }
}