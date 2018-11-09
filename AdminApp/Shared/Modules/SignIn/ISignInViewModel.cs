using System.Reactive;
using ReactiveUI;

namespace TongTongAdmin.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand<Unit, Unit> TriggerGoogleAuthFlow { get; }
    }
}
