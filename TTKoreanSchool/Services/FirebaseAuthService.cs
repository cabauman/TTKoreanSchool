extern alias SplatAlias;

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using SplatAlias::Splat;
using TTKoreanSchool.Config;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using Xamarin.Auth;

namespace TTKoreanSchool.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthProvider _authProvider;
        private readonly IAccountStoreService _accountStoreService;

        private FirebaseAuthLink _authLink;

        public FirebaseAuthService(IAccountStoreService accountStoreService = null)
        {
            _accountStoreService = accountStoreService ?? Locator.Current.GetService<IAccountStoreService>();
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKeys.FIREBASE));

            FirebaseAuthRefreshed
                .Select(firebaseAuth => SaveAccount(firebaseAuth))
                .Subscribe();
        }

        public IObservable<FirebaseAuth> FirebaseAuthRefreshed
        {
            get
            {
                return Observable
                    .FromEventPattern<FirebaseAuthEventArgs>(
                        x => AuthLink.FirebaseAuthRefreshed += x,
                        x => AuthLink.FirebaseAuthRefreshed -= x)
                    .Select(x => x.EventArgs.FirebaseAuth);
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                Account account = AccountStore.Create().FindAccountsForService("Firebase").FirstOrDefault();
                return account != null;
            }
        }

        private FirebaseAuthLink AuthLink { get; set; }

        public async Task<string> GetFreshFirebaseToken()
        {
            return (await AuthLink.GetFreshAuthAsync()).FirebaseToken;
        }

        public IObservable<Unit> SignInWithFacebook(TongTongAccount account)
        {
            return SignInWithOAuth(FirebaseAuthType.Facebook, account);
        }

        public IObservable<Unit> SignInWithGoogle(TongTongAccount account)
        {
            return SignInWithOAuth(FirebaseAuthType.Google, account);
        }

        public IObservable<Unit> SignInAnonymously()
        {
            return _authProvider
                .SignInAnonymouslyAsync()
                .ToObservable()
                .Do(authLink => SetCurrentAccountAndAuthLink(new TongTongAccount(), authLink))
                .SelectMany(authLink => SaveAccount(authLink));
        }

        public void SignOut()
        {
            AuthLink = null;
        }

        private IObservable<Unit> SignInWithOAuth(FirebaseAuthType authType, TongTongAccount account)
        {
            return _authProvider
                .SignInWithOAuthAsync(authType, account.AuthToken)
                .ToObservable()
                .Do(authLink => SetCurrentAccountAndAuthLink(account, authLink))
                .SelectMany(authLink => SaveAccount(authLink));
        }

        private void SetCurrentAccountAndAuthLink(TongTongAccount account, FirebaseAuthLink authLink)
        {
            AuthLink = authLink;
            _accountStoreService.CurrentAccount = account;
        }

        private IObservable<Unit> SaveAccount(FirebaseAuth firebaseAuth)
        {
            string json = JsonConvert.SerializeObject(firebaseAuth);
            return _accountStoreService
                .SaveFirebaseAuthJson(json);
        }
    }
}