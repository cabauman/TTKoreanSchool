using System;
using System.Reactive.Linq;
using Android.App;
using Android.Content;
using ReactiveUI;
using TTKoreanSchool.Services;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Services
{
    public class NavigationService : NavigationServiceBase
    {
        public NavigationService(
            IObservable<Activity> activityCreatedObservable,
            IObservable<Activity> currentActivityObservable,
            bool rootIsNavStack = true,
            IViewLocator viewlocator = null)
                : base(rootIsNavStack, viewlocator)
        {
            activityCreatedObservable
                .Select(activity => activity as IViewFor)
                .Where(viewFor => viewFor != null)
                .Subscribe(viewFor => viewFor.ViewModel = CurrentScreenViewModel);

            currentActivityObservable
                .Where(activity => activity != CurrentActivity)
                .Subscribe(
                    activity =>
                    {
                        CurrentActivity = activity;
                    });
        }

        public Activity CurrentActivity { get; private set; }

        public IScreenViewModel CurrentScreenViewModel { get; private set; }

        protected override void PushScreenNative(IScreenViewModel viewModel, bool resetStack, bool animate)
        {
            CurrentScreenViewModel = viewModel;

            var screen = LocatePageFor<Activity>(viewModel);
            var intent = new Intent(CurrentActivity, screen.GetType());
            if(resetStack)
            {
                // intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            }

            CurrentActivity.StartActivity(intent);
        }

        protected override void PopScreenNative(bool animate)
        {
            CurrentActivity.Finish();
        }

        protected override void PresentScreenNative(IScreenViewModel viewModel, bool animate, Action onComplete, bool withNavStack)
        {
            PushScreenNative(viewModel, false, animate);
            onComplete?.Invoke();
        }

        protected override void DismissScreenNative(bool animate, Action onComplete)
        {
            PopScreenNative(animate);
            onComplete?.Invoke();
        }
    }
}