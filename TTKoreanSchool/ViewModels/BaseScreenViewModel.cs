using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public abstract class BaseScreenViewModel : BaseViewModel, IScreenViewModel
    {
        public void ScreenPopped()
        {
            var navService = Locator.Current.GetService<INavigationService>();
            navService.ScreenPopped();
        }
    }
}