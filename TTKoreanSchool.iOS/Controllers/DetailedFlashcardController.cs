using System;
using System.Drawing;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("DetailedFlashcardController")]
    public class DetailedFlashcardController : ReactiveViewController<IDetailedFlashcardViewModel>
    {
        private DetailedFlashcardKoreanSide _koreanSide;
        private DetailedFlashcardTranslationSide _translationSide;

        public DetailedFlashcardController(int index, IDetailedFlashcardViewModel flashcard)
        {
            Index = index;
        }

        public int Index { get; }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            _koreanSide = DetailedFlashcardKoreanSide.Create();
            _translationSide = DetailedFlashcardTranslationSide.Create();

            View.AddSubview(_koreanSide);
            View.AddSubview(_translationSide);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            _koreanSide.Frame = View.Bounds;
            _translationSide.Frame = View.Bounds;
        }
    }
}