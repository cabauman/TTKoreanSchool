using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCtor.RxNavigation;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TTKS.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterDetailPage : ReactiveMasterDetailPage<MasterDetailViewModel>
    {
        public MasterDetailPage(IViewShell detailView)
        {
            Detail = (Xamarin.Forms.Page)detailView;
            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }

            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.MenuItems, v => v.MyListView.ItemsSource)
                        .DisposeWith(disposables);
                    this
                        .Bind(ViewModel, vm => vm.Selected, v => v.MyListView.SelectedItem)
                        .DisposeWith(disposables);
                    this
                        .WhenAnyValue(x => x.ViewModel.Selected)
                        .Where(x => x != null)
                        .Subscribe(
                            _ =>
                            {
                                // Deselect the cell.
                                Device.BeginInvokeOnMainThread(() => MyListView.SelectedItem = null);
                                // Hide the master panel.
                                IsPresented = false;
                            })
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.SignOut, v => v.SignOutButton)
                        .DisposeWith(disposables);
                });
        }
    }
}