using System;
using Android.App;
using Android.Content;
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
            var intent = new Intent(App.CurrentActivity, screen.GetType());
            if(resetStack)
            {
                // intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            }

            App.CurrentActivity.StartActivity(intent);
        }

        protected override void PopPageNative(bool animate)
        {
            App.CurrentActivity.Finish();
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