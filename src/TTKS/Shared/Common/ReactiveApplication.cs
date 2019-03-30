using ReactiveUI;
using Xamarin.Forms;

namespace TTKS.Common
{
    public class ReactiveApplication : Application, IViewFor<AppBootstrapper>
    {
        public AppBootstrapper ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AppBootstrapper)value;
        }
    }
}
