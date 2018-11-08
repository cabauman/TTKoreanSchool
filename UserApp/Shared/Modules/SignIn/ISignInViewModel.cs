using ReactiveUI;

namespace TTKoreanSchool.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand TriggerGoogleAuthFlow { get; }

        ReactiveCommand TriggerFacebookAuthFlow { get; }
    }
}
