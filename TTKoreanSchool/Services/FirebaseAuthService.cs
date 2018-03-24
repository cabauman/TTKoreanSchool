using System;
using System.Threading.Tasks;
using Firebase.Auth;
using TTKoreanSchool.Config;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthProvider _authProvider;

        private FirebaseAuthLink _authLink;

        public FirebaseAuthService()
        {
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKeys.FIREBASE));
        }

        public static event Action<string> AuthChanged;

        private FirebaseAuthLink AuthLink
        {
            get
            {
                return _authLink;
            }

            set
            {
                _authLink = value;
                AuthChanged?.Invoke(_authLink.FirebaseToken);
            }
        }

        public async Task SignInWithFacebook(string accessToken)
        {
            AuthLink = await _authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, accessToken);
        }

        public async Task SignInWithGoogle(string accessToken)
        {
            AuthLink = await _authProvider.SignInWithOAuthAsync(FirebaseAuthType.Google, accessToken);
        }

        public async Task SignInAnonymously()
        {
            AuthLink = await _authProvider.SignInAnonymouslyAsync();
        }

        public void SignOut()
        {
            AuthLink = null;
        }
    }
}