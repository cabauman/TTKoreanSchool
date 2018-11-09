using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using GameCtor.XamarinAuth;
using ReactiveUI;
using Splat;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class SignInViewModel : BasePageViewModel, ISignInViewModel
    {
        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _provider;

        public SignInViewModel(
            AppBootstrapper appBootstrapper,
            IAuthService authService = null,
            IFirebaseAuthService firebaseAuthService = null)
                : base(null)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();
            authService = authService ?? Locator.Current.GetService<IAuthService>();

            TriggerGoogleAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    //_provider = "google";
                    //authService.TriggerGoogleAuthFlow(
                    //    Config.GoogleAuthConfig.CLIENT_ID_IOS,
                    //    null,
                    //    Config.GoogleAuthConfig.SCOPE,
                    //    Config.GoogleAuthConfig.AUTHORIZE_URL,
                    //    Config.GoogleAuthConfig.REDIRECT_URL_IOS,
                    //    Config.GoogleAuthConfig.ACCESS_TOKEN_URL);
                });

            TriggerGoogleAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    this.Log().Debug(ex);
                });

            //authService.SignInSuccessful
            //    .SelectMany(authToken => AuthenticateWithFirebase(authToken))
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Subscribe(_ => appBootstrapper.MainView = new HomeViewModel(appBootstrapper));
        }

        public override string Title => "Sign In";

        public ReactiveCommand<Unit, Unit> TriggerGoogleAuthFlow { get; }

        private IObservable<Unit> AuthenticateWithFirebase(string authToken)
        {
            IObservable<Unit> result = null;
            if (_provider == "google")
            {
                result = _firebaseAuthService
                    .SignInWithGoogle(null, authToken);
            }

            return result;
        }
    }
}
