﻿using System;
using System.Drawing;
using CoreFoundation;
using Foundation;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("SignInPageController")]
    public class SignInPageController : BaseViewController<ISignInPageViewModel>
    {
        public SignInPageController()
        {
        }

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
            Title = "Sign In";
        }
    }
}