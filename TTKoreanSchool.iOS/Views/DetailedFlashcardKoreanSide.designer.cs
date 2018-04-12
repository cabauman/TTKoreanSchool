// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TTKoreanSchool.iOS
{
    [Register ("DetailedFlashcardKoSide")]
    partial class DetailedFlashcardKoreanSide
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _lblKo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel _lblRomanization { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_lblKo != null) {
                _lblKo.Dispose ();
                _lblKo = null;
            }

            if (_lblRomanization != null) {
                _lblRomanization.Dispose ();
                _lblRomanization = null;
            }
        }
    }
}