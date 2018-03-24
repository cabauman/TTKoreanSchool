extern alias SplatAlias;

using System.Reactive;
using ReactiveUI;
using SplatAlias::Splat;

namespace TTKoreanSchool.ViewModels
{
    public interface IButtonViewModel
    {
        string Title { get; }

        string ImageName { get; }

        IBitmap Bitmap { get; }

        ReactiveCommand<Unit, Unit> Command { get; }
    }

    public class ButtonViewModel : BaseViewModel, IButtonViewModel
    {
        public ButtonViewModel(string title, string imageName, ReactiveCommand<Unit, Unit> command)
        {
            Title = title;
            ImageName = imageName;
            Command = command;
        }

        public string Title { get; }

        public string ImageName { get; }

        public IBitmap Bitmap { get; }

        public ReactiveCommand<Unit, Unit> Command { get; }
    }
}