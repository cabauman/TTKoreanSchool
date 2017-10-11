using System;
using CoreGraphics;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Core;
using Foundation;
using Google.SignIn;
//using SDWebImage;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    public class GoogleSignInViewController : UIViewController, ISignInDelegate, ISignInUIDelegate
    {
        private SignInButton _btnSignIn;
        private UIButton _btnSignOut;
        private UIImageView _imgView;

        public GoogleSignInViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _btnSignIn = new SignInButton();
            _btnSignIn.Frame = new CGRect(20, 20, 150, 44);
            View.AddSubview(_btnSignIn);

            _btnSignOut = new UIButton(UIButtonType.RoundedRect);
            _btnSignIn.Frame = new CGRect(180, 20, 150, 44);
            var btn = new UIButton();
            _btnSignOut.Enabled = false;
            View.AddSubview(_btnSignOut);
            _btnSignOut.TouchUpInside += (sender, e) =>
            {
                SignIn.SharedInstance.SignOutUser();

                _btnSignIn.Enabled = true;
                _btnSignOut.Enabled = false;
            };

            _imgView = new UIImageView(new CGRect(50, 100, 80, 80));
            View.AddSubview(_imgView);

            SignIn.SharedInstance.ClientID = App.DefaultInstance.Options.ClientId;
            SignIn.SharedInstance.Delegate = this;
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.SignInUserSilently();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if(error == null && user != null)
            {
                // Get Google ID token and Google access token and exchange them for a Firebase credential
                var authentication = user.Authentication;
                var credential = GoogleAuthProvider.GetCredential (authentication.IdToken, authentication.AccessToken);

                // Authenticate with Firebase using the credential
                Auth.DefaultInstance.SignIn(credential, SignInOnCompletion);
            }
            else if(error.Code != -4)
            {
                _btnSignIn.Enabled = true;
                AppDelegate.ShowMessage("Could not login!", error.LocalizedDescription + " " + error.LocalizedFailureReason, NavigationController);
            }
        }

        [Export("signIn:didDisconnectWithUser:withError:")]
        public void DidDisconnect(SignIn signIn, GoogleUser user, NSError error)
        {
            // NavigationController.PopToRootViewController(true);
        }

        private void SignInOnCompletion(User user, NSError error)
        {
            if (error != null)
            {
                AuthErrorCode errorCode;
                if(IntPtr.Size == 8)
                {
                    // 64 bits devices
                    errorCode = (AuthErrorCode)((long)error.Code);
                }
                else
                {
                    // 32 bits devices
                    errorCode = (AuthErrorCode)((int)error.Code);
                }

                // Posible error codes that SignIn method with credentials could throw
                // Visit https://firebase.google.com/docs/auth/ios/errors for more information
                switch(errorCode)
                {
                    case AuthErrorCode.InvalidCredential:
                    case AuthErrorCode.InvalidEmail:
                    case AuthErrorCode.OperationNotAllowed:
                    case AuthErrorCode.EmailAlreadyInUse:
                    case AuthErrorCode.UserDisabled:
                    case AuthErrorCode.WrongPassword:
                    default:
                        AppDelegate.ShowMessage("Could not login!", error.LocalizedDescription, NavigationController);
                        break;
                }

                return;
            }

            _btnSignIn.Enabled = false;
            _btnSignOut.Enabled = true;

            //_imgView.SetImage(
            //    url: user.PhotoUrl,
            //    placeholder: UIImage.FromFile("placeholder.png"),
            //    completedBlock: (image, imgError, cacheType, url) =>
            //    {
            //    }
            //);

            // Handle successful Firebase login.
        }
    }
}