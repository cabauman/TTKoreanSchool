using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Disposables;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using TTKoreanSchool.Models;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("DetailedFlashcardSetController")]
    public class DetailedFlashcardSetController : BaseViewController<IDetailedFlashcardSetViewModel>
    {
        private readonly UIPageViewController _pageController;

        public DetailedFlashcardSetController()
        {
            //ViewModel.WhenAnyValue(vm => vm.Terms)
            //    .Subscribe()

            _pageController = new UIPageViewController(
                UIPageViewControllerTransitionStyle.PageCurl,
                UIPageViewControllerNavigationOrientation.Horizontal,
                UIPageViewControllerSpineLocation.Min);

            _pageController.DataSource = new DetailedFlashcardSetDataSource(this);
        }

        public int NumFlashcards
        {
            get { return ViewModel.Terms.Count; }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            Title = "Detailed Flashcards";

            var firstPage = GetFlashcardAtIndex(0);
            var pages = new DetailedFlashcardController[] { firstPage };
            _pageController.SetViewControllers(
                pages,
                UIPageViewControllerNavigationDirection.Forward,
                false,
                null);

            AddChildViewController(_pageController);
            View.AddSubview(_pageController.View);
            _pageController.DidMoveToParentViewController(this);
        }

        public override void ViewDidLayoutSubviews()
        {
            _pageController.View.Frame = View.Bounds;
        }

        public DetailedFlashcardController GetFlashcardAtIndex(int index)
        {
            var flashcardVm = ViewModel.GetFlashcardAtIndex(index);

            return new DetailedFlashcardController(index, flashcardVm);
        }
    }
}