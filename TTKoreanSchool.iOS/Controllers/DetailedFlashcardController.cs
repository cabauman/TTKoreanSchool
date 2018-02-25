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
        private readonly UILabel _lblKo = new UILabel();
        private readonly UILabel _lblRomanization = new UILabel();
        private readonly UILabel _lblTranslation = new UILabel();

        public DetailedFlashcardController(int index, IDetailedFlashcardViewModel flashcard)
        {
            Index = index;

            _lblKo.Text = flashcard.Ko;
            _lblRomanization.Text = flashcard.Romanization;
            _lblTranslation.Text = flashcard.Translation;
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

            var koSide = DetailedFlashcardKoSide.Create();
            koSide.Frame = View.Frame;
            View.AddSubview(koSide);

            var translationSide = DetailedFlashcardTranslationSide.Create();
            translationSide.Frame = View.Frame;
            View.AddSubview(translationSide);
        }
    }
}