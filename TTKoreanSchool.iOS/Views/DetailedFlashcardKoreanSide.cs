using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace TTKoreanSchool.iOS
{
    public partial class DetailedFlashcardKoreanSide : UIView
    {
        private const string NIB_NAME = "DetailedFlashcardKoreanSide";

        public DetailedFlashcardKoreanSide(IntPtr handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static DetailedFlashcardKoreanSide Create()
        {
            var topLevelObjects = NSBundle.MainBundle.LoadNib(NIB_NAME, null, null);
            var view = Runtime.GetNSObject<DetailedFlashcardKoreanSide>(topLevelObjects.ValueAt(0));

            return view;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public void ApplyConstraints(UIView parentView)
        {
            Frame = new CoreGraphics.CGRect(new CoreGraphics.CGPoint(0, 0), parentView.Bounds.Size);
            TranslatesAutoresizingMaskIntoConstraints = false;
            //TranslationLbl.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraints = new[]
            {
                TrailingAnchor.ConstraintEqualTo(parentView.TrailingAnchor, 0f),
                LeadingAnchor.ConstraintEqualTo(parentView.LeadingAnchor, 0f),
                TopAnchor.ConstraintEqualTo(parentView.TopAnchor, 0f),
                BottomAnchor.ConstraintEqualTo(parentView.BottomAnchor, 0f)
            };
            NSLayoutConstraint.ActivateConstraints(constraints);

            //constraints = new[]
            //{
            //    TranslationLbl.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -10f),
            //    TranslationLbl.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 10f),
            //    TranslationLbl.HeightAnchor.ConstraintEqualTo(28f),
            //    TranslationLbl.CenterXAnchor.ConstraintEqualTo(CenterXAnchor),
            //    TranslationLbl.CenterYAnchor.ConstraintEqualTo(CenterYAnchor)
            //};
            //NSLayoutConstraint.ActivateConstraints(constraints);
        }
    }
}