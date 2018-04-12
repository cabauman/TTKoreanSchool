using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace TTKoreanSchool.iOS
{
    public partial class DetailedFlashcardTranslationSide : UIView
    {
        private const string NIB_NAME = "DetailedFlashcardTranslationSide";

        public DetailedFlashcardTranslationSide(IntPtr handle)
            : base(handle)
        {
        }

        public static DetailedFlashcardTranslationSide Create()
        {
            var topLevelObjects = NSBundle.MainBundle.LoadNib(NIB_NAME, null, null);
            var view = Runtime.GetNSObject<DetailedFlashcardTranslationSide>(topLevelObjects.ValueAt(0));

            return view;
        }
    }
}