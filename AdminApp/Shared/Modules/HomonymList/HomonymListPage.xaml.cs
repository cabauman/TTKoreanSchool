using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TongTongAdmin.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomonymListPage : ReactiveContentPage<IHomonymListViewModel>
    {
		public HomonymListPage()
		{
			InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(PopulateFromViewModel);

            this.WhenActivated(Init);
        }

        private void PopulateFromViewModel(IHomonymListViewModel vm)
        {
            //ViewModel.LoadItems.Execute().Subscribe();
            AudiobookListView.ItemsSource = vm.AudiobookItems;
        }

        private void Init(CompositeDisposable disposables)
        {
            this
                .Bind(ViewModel, vm => vm.SelectedItem, v => v.AudiobookListView.SelectedItem)
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
	}
}