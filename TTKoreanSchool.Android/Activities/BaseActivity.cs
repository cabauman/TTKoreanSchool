using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Android.App;
using Android.OS;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "BaseActivity")]
    public class BaseActivity<TViewModel> : ReactiveActivity<TViewModel>, IPageView
        where TViewModel : class, IPageViewModel
    {
        protected CompositeDisposable SubscriptionDisposables { get; } = new CompositeDisposable();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var navService = Locator.Current.GetService<INavigationService>();
            ViewModel = navService.TopPage as TViewModel;
        }

        public override void OnBackPressed()
        {
            ViewModel.PagePopped();
            base.OnBackPressed();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SubscriptionDisposables.Clear();
        }
    }
}