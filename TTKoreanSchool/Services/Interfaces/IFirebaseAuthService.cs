using System;
using System.Reactive;
using System.Threading.Tasks;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        bool IsAuthenticated { get; }

        Task<string> GetFreshFirebaseToken();

        IObservable<Unit> SignInWithFacebook(TongTongAccount account);

        IObservable<Unit> SignInWithGoogle(TongTongAccount account);

        IObservable<Unit> SignInAnonymously();

        void SignOut();
    }
}