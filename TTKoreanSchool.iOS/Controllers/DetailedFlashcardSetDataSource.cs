using System;
using System.Drawing;
using CoreFoundation;
using Foundation;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("DetailedFlashcardSetDataSource")]
    public class DetailedFlashcardSetDataSource : UIPageViewControllerDataSource
    {
        private DetailedFlashcardSetController _owner;

        public DetailedFlashcardSetDataSource(DetailedFlashcardSetController owner)
        {
            _owner = owner;
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var currentPage = referenceViewController as DetailedFlashcardController;
            int index = (currentPage.Index + 1) % _owner.NumFlashcards;

            return _owner.GetFlashcardAtIndex(index);
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var currentPage = referenceViewController as DetailedFlashcardController;
            if(currentPage.Index == 0)
            {
                return _owner.GetFlashcardAtIndex(_owner.NumFlashcards - 1);
            }
            else
            {
                return _owner.GetFlashcardAtIndex(currentPage.Index - 1);
            }
        }
    }
}