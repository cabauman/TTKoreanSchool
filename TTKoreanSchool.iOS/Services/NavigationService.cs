using System;
using ReactiveUI;
using TTKoreanSchool.Services;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Services
{
    public class NavigationService : BaseNavigationService
    {
        public NavigationService(IViewLocator viewlocator = null)
            : base(viewlocator)
        {
            RootViewController = new UINavigationController();
        }

        public UIViewController RootViewController { get; }

        private UIViewController TopMostViewController
        {
            get
            {
                var topVc = RootViewController;

                while(topVc.PresentedViewController != null)
                {
                    topVc = topVc.PresentedViewController;
                }

                return topVc;
            }
        }

        protected override void PushPageNative(IPageViewModel viewModel, bool resetStack, bool animate)
        {
            if(TopMostViewController is UINavigationController navController)
            {
                var page = LocatePageFor<UIViewController>(viewModel);

                if(resetStack)
                {
                    navController.SetViewControllers(new UIViewController[] { page }, true);
                }
                else
                {
                    navController.PushViewController(page, animate);
                }
            }
            else
            {
                throw new InvalidOperationException("Can't push because there isn't a navigation controller on top of the stack.");
            }
        }

        protected override void PopPageNative(bool animate)
        {
            if(TopMostViewController is UINavigationController navController)
            {
                navController.PopViewController(animate);
            }
            else
            {
                throw new InvalidOperationException("Can't pop because there isn't a navigation controller on top of the stack.");
            }
        }

        protected override void PresentPageNative(IPageViewModel viewModel, bool animate, Action onComplete, bool withNavStack)
        {
            var page = LocatePageFor<UIViewController>(viewModel);

            if(withNavStack)
            {
                var navController = new UINavigationController(page);
                page = navController;
            }

            RootViewController.PresentViewController(page, animate, onComplete);
        }

        protected override void DismissPageNative(bool animate, Action onComplete)
        {
            if(RootViewController.PresentedViewController != null)
            {
                RootViewController.DismissViewController(animate, onComplete);
            }
            else
            {
                throw new InvalidOperationException("Can't dismiss because there are no presented view controllers.");
            }
        }
    }
}