using System;
using System.Drawing;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using TTKoreanSchool.Config;
using TTKoreanSchool.Services;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("SignInPageController")]
    public class SignInPageController : BaseViewController<ISignInPageViewModel>
    {
        private UIButton _googleLoginButton;
        private UIButton _facebookLoginButton;

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

            _googleLoginButton = new UIButton();
            _googleLoginButton.SetTitle("Google", UIControlState.Normal);
            _googleLoginButton.BackgroundColor = UIColor.Green;

            _facebookLoginButton = new UIButton();
            _facebookLoginButton.SetTitle("Facebook", UIControlState.Normal);
            _facebookLoginButton.BackgroundColor = UIColor.Blue;

            View.AddSubview(_googleLoginButton);
            View.AddSubview(_facebookLoginButton);

            _googleLoginButton.TouchUpInside += OnGoogleLoginButtonClicked;
            _facebookLoginButton.TouchUpInside += OnFacebookLoginButtonClicked;
        }

        public override void ViewDidLayoutSubviews()
        {
            CGRect screenBounds = UIScreen.MainScreen.Bounds;
            var navBarHeight = NavigationController.NavigationBar.Frame.Height;
            float buttonWidth = (float)screenBounds.Width / 2;
            _googleLoginButton.Frame = new CGRect(0f, navBarHeight, buttonWidth, 50f);
            _facebookLoginButton.Frame = new CGRect(0f, navBarHeight + 60f, buttonWidth, 50f);
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            DismissViewController(true, null);
        }

        public void OnAuthenticationCanceled()
        {
            DismissViewController(true, null);
        }

        private void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
        }
    }
}