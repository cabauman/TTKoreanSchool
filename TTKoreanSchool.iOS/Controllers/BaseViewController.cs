using System;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("BaseViewController")]
    public class BaseViewController<TViewModel> : ReactiveViewController<TViewModel>, IPageView
        where TViewModel : class, IPageViewModel
    {
        public BaseViewController()
        {
        }

        protected CompositeDisposable SubscriptionDisposables { get; } = new CompositeDisposable();

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            SubscriptionDisposables.Clear();
        }

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);

            if(parent == null)
            {
                ViewModel.PagePopped();
            }
        }
    }
}