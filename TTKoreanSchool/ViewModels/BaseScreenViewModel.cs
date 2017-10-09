using System;

namespace TTKoreanSchool.ViewModels
{
    public interface IScreenView
    {
        IObservable<IScreenViewModel> PagePopped { get; }
    }
}
