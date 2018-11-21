using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TongTongAdmin.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VocabListPage : ReactiveContentPage<IVocabListViewModel>
    {
        private SfDataGrid _sfGrid;

		public VocabListPage()
		{
			InitializeComponent();

            Device.BeginInvokeOnMainThread(
                () =>
                {
                    _sfGrid = new SfDataGrid();
                    _sfGrid.ColumnSizer = ColumnSizer.Star;
                    _sfGrid.VerticalOptions = LayoutOptions.FillAndExpand;
                    _sfGrid.NavigationMode = NavigationMode.Cell;
                    _sfGrid.SelectionMode = SelectionMode.Single;
                    _sfGrid.AllowEditing = true;
                    _sfGrid.EditTapAction = TapAction.OnDoubleTap;
                    _sfGrid.EditorSelectionBehavior = EditorSelectionBehavior.MoveLast;
                    _sfGrid.ItemsSource = new Services.OrderInfoRepository().OrderInfoCollection;

                    Observable.FromEventPattern<AutoGeneratingColumnEventHandler, AutoGeneratingColumnEventArgs>(
                        h => _sfGrid.AutoGeneratingColumn += h,
                        h => _sfGrid.AutoGeneratingColumn -= h)
                            .Select(x => x.EventArgs)
                            .Subscribe(AdjustColumnSettings);

                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children = { _sfGrid },
                    };

                    this
                        .WhenAnyValue(x => x.ViewModel)
                        .Where(x => x != null)
                        .Take(1)
                        .Subscribe(PopulateFromViewModel);

                    this.WhenActivated(Init);
                });
        }

        private void PopulateFromViewModel(IVocabListViewModel vm)
        {
            //ViewModel.LoadItems.Execute().Subscribe();
            _sfGrid.ItemsSource = vm.Items;
        }

        private void Init(CompositeDisposable disposables)
        {
            this
                .Bind(ViewModel, vm => vm.SelectedItem, v => v._sfGrid.SelectedItem)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.CreateItem, v => v.AddButton)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.SaveItem, v => v.SaveButton)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.DeleteItem, v => v.DeleteButton)
                .DisposeWith(disposables);
            this
                .ViewModel
                .ConfirmDelete
                .RegisterHandler(
                    async context =>
                    {
                        bool result = await DisplayAlert("Delete Image", $"Are you sure you want to delete '{context.Input}'?", "Yes", "No");
                        context.SetOutput(result);
                    })
                .DisposeWith(disposables);
        }

        private void AdjustColumnSettings(AutoGeneratingColumnEventArgs eventArgs)
        {
            if (eventArgs.Column.MappingName == nameof(ViewModel.SelectedItem.Model))
            {
                eventArgs.Cancel = true;
            }
            else if (eventArgs.Column.MappingName == nameof(ViewModel.SelectedItem.ImageIds) ||
                eventArgs.Column.MappingName == nameof(ViewModel.SelectedItem.SentenceIds))
            {
                eventArgs.Column.AllowEditing = false;
            }
        }
    }
}