using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("DetailedFlashcardSetController")]
    public class DetailedFlashcardSetController : BaseViewController<IDetailedFlashcardsPageViewModel>
    {
        private readonly UIPageViewController _pageController;

        public DetailedFlashcardSetController()
        {
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

            ViewModel.WhenAnyValue(vm => vm.LoadSentences)
                .SelectMany(x => x.Execute())
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    sentences =>
                    {
                        this.Log().Debug("{0} sentences loaded.", sentences.Count);
                    },
                    ex =>
                    {
                        this.Log().Debug(ex);
                    })
                .DisposeWith(SubscriptionDisposables);
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