using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Android.App;
using Android.OS;
using ReactiveUI;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "BaseActivity")]
    public class BaseActivity<TViewModel> : ReactiveActivity<TViewModel>, IScreenView
        where TViewModel : class, IScreenViewModel
    {
        private readonly Subject<IScreenViewModel> _pagePopped = new Subject<IScreenViewModel>();

        public IObservable<IScreenViewModel> PagePopped
        {
            get { return _pagePopped as IObservable<IScreenViewModel>; }
        }

        protected CompositeDisposable SubscriptionDisposables { get; } = new CompositeDisposable();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnBackPressed()
        {
            _pagePopped.OnNext(ViewModel);
            _pagePopped.OnCompleted();
            base.OnBackPressed();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SubscriptionDisposables.Clear();
        }
    }
}