using System;
using System.Threading.Tasks;
using Firebase.Auth;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        Task SignInWithFacebook(string accessToken);

        Task SignInWithGoogle(string accessToken);

        Task SignInAnonymously();

        void SignOut();
    }
}