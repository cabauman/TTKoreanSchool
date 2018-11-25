using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TongTongAdmin.Modules
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
        }
	}
}