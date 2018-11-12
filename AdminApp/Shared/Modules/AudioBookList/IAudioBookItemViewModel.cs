using System.Reactive;
using Plugin.Media.Abstractions;
using Plugin.MediaManager.Abstractions;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IAudiobookItemViewModel
    {
        Audiobook Model { get; set; }

        ReactiveCommand<Unit, Unit> PlayAudio { get; }

        ReactiveCommand<Unit, string> PlayAudio2 { get; }

        ReactiveCommand<Unit, Unit> StopAudio { get; }

        ReactiveCommand<Unit, MediaFile> UploadImage { get; }

        ReactiveCommand<Unit, Unit> DeleteImage { get; }

        ReactiveCommand<Unit, Unit> UploadAudio { get; }

        Interaction<string, bool> ConfirmDelete { get; }

        IMediaFile AudioFile { get; set; }

        string ImageUrl { get; set; }
    }
}
