extern alias SplatAlias;

using System.Threading.Tasks;
using SplatAlias::Splat;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface ISignInPageViewModel : IPageViewModel
    {
    }

    public class SignInPageViewModel : BasePageViewModel, ISignInPageViewModel
    {
        private IFirebaseAuthService _authService;

        public SignInPageViewModel(IFirebaseAuthService authService = null)
        {
            _authService = authService ?? Locator.Current.GetService<IFirebaseAuthService>();
        }

        public async Task SignInWithFacebook(string accessToken)
        {
            await _authService.SignInWithFacebook(accessToken);
        }

        public async Task SignInWithGoogle(string accessToken)
        {
            await _authService.SignInWithGoogle(accessToken);
        }

        public async Task SignInAnonymously()
        {
            await _authService.SignInAnonymously();
        }
    }
}