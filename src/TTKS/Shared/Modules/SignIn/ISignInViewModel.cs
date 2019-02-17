using System.Reactive;
using ReactiveUI;

namespace TTKS.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand<Unit, Unit> TriggerGoogleAuthFlow { get; }

        ReactiveCommand<Unit, Unit> TriggerFacebookAuthFlow { get; }

        ReactiveCommand<Unit, Unit> ContinueAnonymously { get; }
    }
}
