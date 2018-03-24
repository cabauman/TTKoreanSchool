using System;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface INavigationService
    {
        IPageViewModel TopPage { get; }

        void PushPage(IPageViewModel viewModel, bool resetStack = false, bool animate = true);

        void PopPage(bool animate = true);

        void PresentPage(IPageViewModel viewModel, bool animate = true, Action onComplete = null, bool withNavStack = false);

        void DismissPage(bool animate, Action onComplete);

        void PagePopped();
    }
}