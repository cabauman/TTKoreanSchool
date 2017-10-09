using System.Reactive;
using ReactiveUI;

namespace TTKoreanSchool.ViewModels
{
    public interface IAppSectionViewModel
    {
        string Title { get; }

        string ImageName { get; }

        ReactiveCommand<Unit, Unit> GoToSection { get; }
    }

    public class AppSectionViewModel : BaseViewModel
    {
        public AppSectionViewModel(string title, string imageName, ReactiveCommand<Unit, Unit> goToSection)
        {
            Title = title;
            ImageName = imageName;
            GoToSection = goToSection;
        }

        public string Title { get; }

        public string ImageName { get; }

        public ReactiveCommand<Unit, Unit> GoToSection { get; }
    }
}
