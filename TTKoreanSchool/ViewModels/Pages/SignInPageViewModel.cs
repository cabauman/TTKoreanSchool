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

        ReactiveCommand<Unit, Unit> ContinueAsGuest { get; }

        IObservable<Unit> SignInSuccessful { get; }

        IObservable<Unit> SignInCanceled { get; }

        IObservable<AuthenticatorErrorEventArgs> SignInFailed { get; }

        WebRedirectAuthenticator Authenticator { get; }
    }

    public class SignInPageViewModel : BasePageViewModel, ISignInPageViewModel
    {
        private readonly INavigationService _navService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private ObservableAsPropertyHelper<WebRedirectAuthenticator> _authenticator;
        private IObservable<Unit> _signInSuccessful;
        private IObservable<Unit> _signInCanceled;
        private IObservable<AuthenticatorErrorEventArgs> _signInFailed;

        private string _provider;

        public SignInPageViewModel(INavigationService navService = null, IFirebaseAuthService authService = null)
        {
            _navService = navService ?? Locator.Current.GetService<INavigationService>();
            _firebaseAuthService = authService ?? Locator.Current.GetService<IFirebaseAuthService>();

            ContinueAsGuest = ReactiveCommand.CreateFromObservable(() => SignInAsGuest());
            ContinueAsGuest
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => _navService.PushPage(new HomePageViewModel(), true));

            ContinueAsGuest.ThrownExceptions.Subscribe(
                ex =>
                {
                    Console.WriteLine(ex);
                });

            TriggerGoogleAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    if(_provider == "Google")
                    {
                        return Authenticator;
                    }

                    _provider = "Google";

                    string clientId = GoogleAuthConfig.CLIENT_ID_ANDROID;
                    string redirectUrl = GoogleAuthConfig.REDIRECT_URL_ANDROID;
                    if(CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.iOS)
                    {
                        clientId = GoogleAuthConfig.CLIENT_ID_IOS;
                        redirectUrl = GoogleAuthConfig.REDIRECT_URL_IOS;
                    }

                    var authenticator = new OAuth2Authenticator(
                        clientId,
                        string.Empty,
                        GoogleAuthConfig.SCOPE,
                        new Uri(GoogleAuthConfig.AUTHORIZE_URL),
                        new Uri(redirectUrl),
                        new Uri(GoogleAuthConfig.ACCESS_TOKEN_URL),
                        null,
                        true);

                    Observe(authenticator);

                    return authenticator;
                });

            TriggerGoogleAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    Console.WriteLine(ex);
                });

            _authenticator = this.WhenAnyObservable(x => x.TriggerGoogleAuthFlow)
                .ToProperty(this, nameof(Authenticator));

            TriggerFacebookAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    if(_provider == "Facebook")
                    {
                        return Authenticator;
                    }

                    _provider = "Facebook";

                    var authenticator = new OAuth2Authenticator(
                        FacebookAuthConfig.CLIENT_ID,
                        FacebookAuthConfig.SCOPE,
                        new Uri(FacebookAuthConfig.AUTHORIZE_URL),
                        new Uri(FacebookAuthConfig.REDIRECT_URL),
                        null,
                        true);

                    Observe(authenticator);

                    return authenticator;
                });

            TriggerFacebookAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    Console.WriteLine(ex);
                });

            _authenticator = this.WhenAnyObservable(x => x.TriggerFacebookAuthFlow)
                .ToProperty(this, nameof(Authenticator));

            this.WhenAnyObservable(x => x.SignInSuccessful)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    _ =>
                    {
                        _navService.PushPage(new HomePageViewModel(), true);
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

        public ReactiveCommand<Unit, Unit> ContinueAsGuest { get; }

        public IObservable<Unit> SignInSuccessful
        {
            get { return _signInSuccessful; }
            private set { this.RaiseAndSetIfChanged(ref _signInSuccessful, value); }
        }

        public IObservable<Unit> SignInCanceled
        {
            get { return _signInCanceled; }
            private set { this.RaiseAndSetIfChanged(ref _signInCanceled, value); }
        }

        public IObservable<AuthenticatorErrorEventArgs> SignInFailed
        {
            get { return _signInFailed; }
            private set { this.RaiseAndSetIfChanged(ref _signInFailed, value); }
        }

        public WebRedirectAuthenticator Authenticator
        {
            get { return _authenticator.Value; }
        }

        private void Observe(WebRedirectAuthenticator auth)
        {
            var authCompleted = Observable.FromEventPattern<AuthenticatorCompletedEventArgs>(
                x => auth.Completed += x,
                x => auth.Completed -= x);

            SignInSuccessful = authCompleted
                .Where(x => x.EventArgs.IsAuthenticated)
                .Select(x => x.EventArgs.Account)
                .Select(authAccount => ConvertToTongTongAccount(authAccount))
                .SelectMany(ttAccount => AuthenticateWithFirebase(ttAccount));

            SignInCanceled = authCompleted
                .Where(x => !x.EventArgs.IsAuthenticated)
                .Select(_ => Unit.Default);

            SignInFailed = Observable.FromEventPattern<AuthenticatorErrorEventArgs>(
                x => auth.Error += x,
                x => auth.Error -= x)
                    .Select(x => x.EventArgs);
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

        private IObservable<Unit> SignInAsGuest()
        {
            return _firebaseAuthService
                .SignInAnonymously();
        }
    }
}