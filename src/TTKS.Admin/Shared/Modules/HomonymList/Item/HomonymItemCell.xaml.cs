using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms.Xaml;

namespace TTKS.Admin.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomonymItemCell : ReactiveContentView<IHomonymItemViewModel>
    {
		public HomonymItemCell()
		{
			InitializeComponent();

            this.WhenActivated(Init);
        }

        private void Init(CompositeDisposable disposables)
        {
            Observable.FromEventPattern<Xamarin.Forms.FocusEventArgs>(
                h => TextEntry.Focused += h,
                h => TextEntry.Focused -= h)
                    .SelectMany(_ => Observable.Return(false).StartWith(true))
                    .BindTo(this, x => x.ViewModel.ReceivedFocus);

            this
                .Bind(ViewModel, vm => vm.Text, v => v.TextEntry.Text)
                .DisposeWith(disposables);
        }
	}
}