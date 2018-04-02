extern alias SplatAlias;

using System;
using System.Reactive;
using System.Reactive.Linq;
using Plugin.DeviceInfo;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Config;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using Xamarin.Auth;

namespace TTKoreanSchool.ViewModels
{
    public interface ISignInPageViewModel : IPageViewModel
    {
        ReactiveCommand<Unit, WebRedirectAuthenticator> TriggerGoogleAuthFlow { get; }

        ReactiveCommand<Unit, WebRedirectAuthenticator> TriggerFacebookAuthFlow { get; }

        ReactiveCommand ContinueAsGuest { get; }

        IObservable<Unit> SignInSuccessful { get; }

        IObservable<Unit> SignInCanceled { get; }

        IObservable<AuthenticatorErrorEventArgs> SignInFailed { get; }

        WebRedirectAuthenticator Authenticator { get; }
    }

    public class SignInPageViewModel : BasePageViewModel, ISignInPageViewModel
    {
        private readonly INavigationService _navService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly ObservableAsPropertyHelper<WebRedirectAuthenticator> _authenticator;

        private string _provider;

        public SignInPageViewModel(INavigationService navService = null, IFirebaseAuthService authService = null)
        {
            _navService = navService ?? Locator.Current.GetService<INavigationService>();
            _firebaseAuthService = authService ?? Locator.Current.GetService<IFirebaseAuthService>();

            var completed = Observable.FromEventPattern<AuthenticatorCompletedEventArgs>(
                x => Authenticator.Completed += x,
                x => Authenticator.Completed -= x);

            SignInSuccessful = completed
                .Where(x => x.EventArgs.IsAuthenticated)
                .Select(x => x.EventArgs.Account)
                .Select(authAccount => ConvertToTongTongAccount(authAccount))
                .SelectMany(ttAccount => AuthenticateWithFirebase(ttAccount));

            SignInCanceled = completed
                .Where(x => !x.EventArgs.IsAuthenticated)
                .Select(_ => Unit.Default);

            SignInFailed = Observable.FromEventPattern<AuthenticatorErrorEventArgs>(
                x => Authenticator.Error += x,
                x => Authenticator.Error -= x)
                    .Select(x => x.EventArgs);

            ContinueAsGuest = ReactiveCommand.CreateFromObservable(() => _firebaseAuthService.SignInAnonymously());

            TriggerGoogleAuthFlow = ReactiveCommand.Create<WebRedirectAuthenticator>(
                () =>
                {
                    _provider = "Google";

                    string clientId = GoogleAuthConfig.CLIENT_ID_ANDROID;
                    string redirectUrl = GoogleAuthConfig.REDIRECT_URL_ANDROID;
                    if(CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.iOS)
                    {
                        clientId = GoogleAuthConfig.CLIENT_ID_IOS;
                        redirectUrl = GoogleAuthConfig.REDIRECT_URL_IOS;
                    }

                    var oAuth2 = new OAuth2Authenticator(
                        clientId,
                        string.Empty,
                        GoogleAuthConfig.SCOPE,
                        new Uri(GoogleAuthConfig.AUTHORIZE_URL),
                        new Uri(redirectUrl),
                        new Uri(GoogleAuthConfig.ACCESS_TOKEN_URL),
                        null,
                        true);

                    return oAuth2;
                });

            TriggerFacebookAuthFlow = ReactiveCommand.Create<WebRedirectAuthenticator>(
                () =>
                {
                    _provider = "Facebook";

                    var oAuth2 = new OAuth2Authenticator(
                        FacebookAuthConfig.CLIENT_ID,
                        FacebookAuthConfig.SCOPE,
                        new Uri(FacebookAuthConfig.AUTHORIZE_URL),
                        new Uri(FacebookAuthConfig.REDIRECT_URL),
                        null,
                        true);

                    return oAuth2;
                });

            TriggerGoogleAuthFlow.ToProperty(this, vm => vm.Authenticator, out _authenticator);
            TriggerFacebookAuthFlow.ToProperty(this, vm => vm.Authenticator, out _authenticator);

            SignInSuccessful
                .Subscribe(
                    _ =>
                    {
                        navService.PushPage(new HomePageViewModel(), true);
                    },
                    ex =>
                    {
                        Console.WriteLine(ex);
                    },
                    () =>
                    {
                        Console.WriteLine("Complete");
                    });
        }

        public ReactiveCommand<Unit, WebRedirectAuthenticator> TriggerGoogleAuthFlow { get; private set; }

        public ReactiveCommand<Unit, WebRedirectAuthenticator> TriggerFacebookAuthFlow { get; }

        public ReactiveCommand ContinueAsGuest { get; }

        public IObservable<Unit> SignInSuccessful { get; }

        public IObservable<Unit> SignInCanceled { get; }

        public IObservable<AuthenticatorErrorEventArgs> SignInFailed { get; }

        public WebRedirectAuthenticator Authenticator
        {
            get { return _authenticator.Value; }
        }

        public void OnPageLoading(Uri uri)
        {
            Authenticator.OnPageLoading(uri);
        }

        private TongTongAccount ConvertToTongTongAccount(Xamarin.Auth.Account account)
        {
            if(Authenticator.GetType() == typeof(OAuth2Authenticator))
            {
                account.Properties["AuthToken"] = account.Properties["access_token"];
            }
            else
            {
                account.Properties["AuthToken"] = account.Properties["oauth_token"];
            }

            return new TongTongAccount(account);
        }

        private IObservable<Unit> AuthenticateWithFirebase(TongTongAccount account)
        {
            IObservable<Unit> result = null;
            if(_provider == "Google")
            {
                result = _firebaseAuthService
                    .SignInWithGoogle(account);
            }
            else if(_provider == "Facebook")
            {
                result = _firebaseAuthService
                    .SignInWithFacebook(account);
            }

            return result;
        }
    }
}