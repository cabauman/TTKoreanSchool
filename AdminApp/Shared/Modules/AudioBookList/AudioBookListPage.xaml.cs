using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.XamForms;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TongTongAdmin.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AudioBookListPage : ReactiveContentPage<IAudioBookListViewModel>
    {
		public AudioBookListPage()
		{
			InitializeComponent();

            Device.BeginInvokeOnMainThread(
                () =>
                {
                    var sfGrid = new SfDataGrid();
                    sfGrid.ColumnSizer = ColumnSizer.Star;
                    sfGrid.VerticalOptions = LayoutOptions.FillAndExpand;
                    sfGrid.NavigationMode = NavigationMode.Cell;
                    sfGrid.SelectionMode = SelectionMode.Single;
                    sfGrid.AllowEditing = true;
                    sfGrid.EditTapAction = TapAction.OnDoubleTap;
                    sfGrid.EditorSelectionBehavior = EditorSelectionBehavior.MoveLast;
                    sfGrid.ItemsSource = new Services.OrderInfoRepository().OrderInfoCollection;

                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children = { sfGrid },
                    };
                });
        }
	}
}