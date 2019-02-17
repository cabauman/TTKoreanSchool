using System.Reactive;
using ReactiveUI;

namespace TTKS.Admin.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand<Unit, Unit> TriggerGoogleAuthFlow { get; }
    }
}
