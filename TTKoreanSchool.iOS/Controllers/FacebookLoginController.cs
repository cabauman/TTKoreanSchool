using System;
using System.Collections.Generic;
using CoreGraphics;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Firebase.Auth;
using Foundation;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    public class FacebookLoginController : UIViewController
    {
        // This permission is set by default, even if you don't add it, but FB recommends to add it anyway
        private List<string> _readPermissions = new List<string> { "public_profile" };

        private UILabel _nameLabel;

        public FacebookLoginController()
        {
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Profile.Notifications.ObserveDidChange((sender, e) =>
            {
                if(e.NewProfile == null)
                {
                    return;
                }

                _nameLabel.Text = e.NewProfile.Name;
            });

            // Set the Read and Publish permissions you want to get
            LoginButton loginBtn = new LoginButton(new CGRect(51, 0, 218, 46))
            {
                LoginBehavior = LoginBehavior.Native,
                ReadPermissions = _readPermissions.ToArray()
            };

            loginBtn.Completed += BtnLogin_Completed;
            loginBtn.ReadPermissions = _readPermissions.ToArray();

            // Handle actions once the user is logged out
            loginBtn.LoggedOut += (sender, e) =>
            {
                // Handle your logout
            };

            if(AccessToken.CurrentAccessToken != null)
            {
                await SignInWithFacebook(AccessToken.CurrentAccessToken.TokenString);
                //var credential = FacebookAuthProvider.GetCredential(AccessToken.CurrentAccessToken.TokenString);
                //Auth.DefaultInstance.SignIn(credential, SignInOnCompletion);
            }

            // The user image profile is set automatically once is logged in
            var pictureView = new ProfilePictureView(new CGRect(50, 50, 220, 220));

            // Create the label that will hold user's facebook name
            _nameLabel = new UILabel(new CGRect(20, 319, 280, 21))
            {
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear
            };

            // Add views to main view
            View.AddSubview(loginBtn);
            View.AddSubview(pictureView);
            View.AddSubview(_nameLabel);
        }

        public async System.Threading.Tasks.Task SignInWithFacebook(string accessToken)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(""));
            var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, accessToken);
        }

        public async System.Threading.Tasks.Task SignInWithGoogle(string accessToken)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(""));
            var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Google, accessToken);
        }

        public async System.Threading.Tasks.Task SignInAnonymously()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(""));
            var auth = await authProvider.SignInAnonymouslyAsync();
        }

        private void BtnLogin_Completed(object sender, LoginButtonCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                // Handle if there was an error
                AppDelegate.ShowMessage("Could not login!", e.Error.Description, this);
                return;
            }

            if(e.Result.IsCancelled)
            {
                // Handle if the user cancelled the login request
                AppDelegate.ShowMessage("Could not login!", "The user cancelled the login", this);
                return;
            }

            // Get access token for the signed-in user and exchange it for a Firebase credential
            //var credential = FacebookAuthProvider.GetCredential(AccessToken.CurrentAccessToken.TokenString);

            // Authenticate with Firebase using the credential
            //Auth.DefaultInstance.SignIn(credential, SignInOnCompletion);
        }

        //private void SignInOnCompletion(User user, NSError error)
        //{
        //    if(error != null)
        //    {
        //        AuthErrorCode errorCode;
        //        if(IntPtr.Size == 8)
        //        {
        //            // 64 bits devices
        //            errorCode = (AuthErrorCode)((long)error.Code);
        //        }
        //        else
        //        {
        //            // 32 bits devices
        //            errorCode = (AuthErrorCode)((int)error.Code);
        //        }

        //        // Posible error codes that SignIn method with credentials could throw
        //        // Visit https://firebase.google.com/docs/auth/ios/errors for more information
        //        switch(errorCode)
        //        {
        //            case AuthErrorCode.InvalidCredential:
        //            case AuthErrorCode.InvalidEmail:
        //            case AuthErrorCode.OperationNotAllowed:
        //            case AuthErrorCode.EmailAlreadyInUse:
        //            case AuthErrorCode.UserDisabled:
        //            case AuthErrorCode.WrongPassword:
        //            default:
        //                AppDelegate.ShowMessage("Could not login!", error.LocalizedDescription, NavigationController);
        //                break;
        //        }

        //        return;
        //    }

        //    // Handle successful Firebase login.
        //}

        public class Acct
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public bool IsAdmin { get; set; }
        }
    }
}