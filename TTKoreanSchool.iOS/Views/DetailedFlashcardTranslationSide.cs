using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace TTKoreanSchool.iOS
{
    public partial class DetailedFlashcardTranslationSide : UIView
    {
        public DetailedFlashcardTranslationSide(IntPtr handle)
            : base(handle)
        {
        }

        public static DetailedFlashcardTranslationSide Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("DetailedFlashcardTranslationSide", null, null);
            var v = Runtime.GetNSObject<DetailedFlashcardTranslationSide>(arr.ValueAt(0));

            return v;
        }
    }
}