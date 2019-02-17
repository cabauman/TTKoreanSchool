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

namespace TTKS.Admin.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AudiobookItemCell : ReactiveContentView<IAudiobookItemViewModel>
    {
		public AudiobookItemCell()
		{
			InitializeComponent();

            this.WhenActivated(Init);
        }

        private void Init(CompositeDisposable disposables)
        {
            this
                .BindCommand(ViewModel, vm => vm.PlayAudio, v => v.PlayAudioButton)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.StopAudio, v => v.StopAudioButton)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.UploadAudio, v => v.UploadAudioButton)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.DeleteAudio, v => v.DeleteAudioButton)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.UploadImage, v => v.EditImageButton)
                .DisposeWith(disposables);
            this
                .BindCommand(ViewModel, vm => vm.DeleteImage, v => v.DeleteImageButton)
                .DisposeWith(disposables);
            this
                .OneWayBind(ViewModel, vm => vm.IsPlaying, v => v.PlayAudioButton.IsVisible, playing => !playing)
                .DisposeWith(disposables);
            this
                .OneWayBind(ViewModel, vm => vm.IsPlaying, v => v.StopAudioButton.IsVisible)
                .DisposeWith(disposables);
            this
                .Bind(ViewModel, vm => vm.Title, v => v.TitleEntry.Text)
                .DisposeWith(disposables);
        }
	}
}