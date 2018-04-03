using System;
using Android.App;
using Android.Content;
using Plugin.CurrentActivity;
using TTKoreanSchool.Services;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Services
{
    public class NavigationService : BaseNavigationService
    {
        public IPageViewModel CurrentPageViewModel { get; private set; }

        protected override void PushPageNative(IPageViewModel viewModel, bool resetStack, bool animate)
        {
            CurrentPageViewModel = viewModel;

            var screen = LocatePageFor<Activity>(viewModel);
            var intent = new Intent(CrossCurrentActivity.Current.Activity, screen.GetType());
            if(resetStack)
            {
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                //intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            }

            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        protected override void PopPageNative(bool animate)
        {
            CrossCurrentActivity.Current.Activity.Finish();
        }

        protected override void PresentPageNative(IPageViewModel viewModel, bool animate, Action onComplete, bool withNavStack)
        {
            PushPageNative(viewModel, false, animate);
            onComplete?.Invoke();
        }

        protected override void DismissPageNative(bool animate, Action onComplete)
        {
            PopPageNative(animate);
            onComplete?.Invoke();
        }
    }
}