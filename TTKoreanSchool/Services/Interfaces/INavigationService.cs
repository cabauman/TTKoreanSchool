using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface INavigationService
    {
        void PushScreen(IScreenViewModel viewModel, bool resetStack = false, bool animate = true);

        void PopScreen(bool animate = true);

        void PresentScreen(IScreenViewModel viewModel, bool animate = true, Action onComplete = null, bool withNavStack = false);

        void DismissScreen(bool animate, Action onComplete);

        void ScreenPopped();
    }
}
