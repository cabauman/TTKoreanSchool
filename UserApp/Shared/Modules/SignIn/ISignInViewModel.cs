using System.Reactive;
using ReactiveUI;

namespace TTKoreanSchool.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand<Unit, Unit> TriggerGoogleAuthFlow { get; }

        ReactiveCommand<Unit, Unit> TriggerFacebookAuthFlow { get; }

        ReactiveCommand<Unit, Unit> ContinueAnonymously { get; }
    }
}
