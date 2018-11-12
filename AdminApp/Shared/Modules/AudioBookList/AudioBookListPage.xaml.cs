using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TongTongAdmin.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AudiobookListPage : ReactiveContentPage<IAudiobookListViewModel>
    {
		public AudiobookListPage()
		{
			InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x => AudiobookListView.ItemsSource = x.AudiobookItems);
            //this
            //    .Bind(ViewModel, vm => vm.AudiobookItems, v => v.AudiobookListView.ItemsSource);
            this
                .BindCommand(ViewModel, vm => vm.CreateItem, v => v.AddButton);
            this
                .BindCommand(ViewModel, vm => vm.SaveItem, v => v.SaveButton);
            this
                .BindCommand(ViewModel, vm => vm.DeleteItem, v => v.DeleteButton);

            this
                .ViewModel
                .ConfirmDelete
                .RegisterHandler(
                    async context =>
                    {
                        bool result = await DisplayAlert("Delete Image", $"Are you sure you want to delete '{context.Input}'?", "Yes", "No");
                        context.SetOutput(result);
                    });
        }
	}
}