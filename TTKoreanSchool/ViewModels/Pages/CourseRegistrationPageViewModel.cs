using ReactiveUI;

namespace TTKoreanSchool.ViewModels
{
    public interface ICourseRegistrationPageViewModel : IPageViewModel
    {
    }

    public class CourseRegistrationPageViewModel : BasePageViewModel, ICourseRegistrationPageViewModel
    {
        private string _courseTitle;

        public CourseRegistrationPageViewModel()
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