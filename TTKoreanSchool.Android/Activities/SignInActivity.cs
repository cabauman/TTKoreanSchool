using System;
using System.Reactive.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using ReactiveUI;
using TTKoreanSchool.ViewModels;
using Xamarin.Auth;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "SignInActivity")]
    public class SignInActivity : BaseActivity<ISignInPageViewModel>
    {
        private Button _googleSignInBtn;
        private Button _facebookSignInBtn;
        private Button _guestSignInBtn;

        public static WebRedirectAuthenticator Authenticator { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Activity_SignIn);

            _googleSignInBtn = FindViewById<Button>(Resource.Id.googleSignInButton);
            this.BindCommand(
                ViewModel,
                vm => vm.TriggerGoogleAuthFlow,
                v => v._googleSignInBtn);

            _facebookSignInBtn = FindViewById<Button>(Resource.Id.facebookSignInButton);
            this.BindCommand(
                ViewModel,
                vm => vm.TriggerFacebookAuthFlow,
                v => v._facebookSignInBtn);

            _guestSignInBtn = FindViewById<Button>(Resource.Id.guestSignInButton);
            this.BindCommand(
                ViewModel,
                vm => vm.ContinueAsGuest,
                v => v._guestSignInBtn);

            this.WhenAnyValue(v => v.ViewModel.Authenticator)
                .Where(auth => auth != null)
                .Subscribe(PresentSignInUI);

            this.WhenAnyObservable(x => x.ViewModel.SignInCanceled)
                .Subscribe(_ => OnAuthenticationCanceled());

            this.WhenAnyObservable(x => x.ViewModel.SignInFailed)
                .Subscribe(args => OnAuthenticationFailed(args.Message, args.Exception));
        }

        private void OnAuthenticationCanceled()
        {
            new AlertDialog.Builder(this)
                .SetTitle("Authentication canceled")
                .SetMessage("You didn't completed the authentication process")
                .Show();
        }

        private void OnAuthenticationFailed(string message, Exception exception)
        {
            new AlertDialog.Builder(this)
                .SetTitle(message)
                .SetMessage(exception?.ToString())
                .Show();
        }

        private void PresentSignInUI(WebRedirectAuthenticator authenticator)
        {
            Authenticator = authenticator;
            var intent = authenticator.GetUI(this);
            CustomTabsConfiguration.CustomTabsClosingMessage = null;
            StartActivity(intent);
        }
    }
}