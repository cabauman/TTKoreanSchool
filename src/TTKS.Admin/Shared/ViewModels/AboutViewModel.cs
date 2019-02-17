using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using Xamarin.Forms;

namespace TTKS.Admin.ViewModels
{
    public class AboutViewModel : BaseViewModel, IEnableLogger
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}