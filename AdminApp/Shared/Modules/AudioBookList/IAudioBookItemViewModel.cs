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

        ReactiveCommand<Unit, Unit> StopAudio { get; }

        ReactiveCommand<Unit, MediaFile> UploadImage { get; }

        ReactiveCommand<Unit, Unit> UploadAudio { get; }

        IMediaFile AudioFile { get; set; }

        string ImageUrl { get; set; }
    }
}
