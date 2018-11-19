using System.Reactive;
using GameCtor.FirebaseStorage.DotNet;
using Plugin.MediaManager.Abstractions;
using Plugin.MediaManager.Reactive;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IAudiobookItemViewModel
    {
        Audiobook Model { get; set; }

        ReactiveMediaManager MediaManager { get; }

        ReactiveCommand<Unit, Unit> PlayAudio { get; }

        ReactiveCommand<Unit, Unit> StopAudio { get; }

        bool IsPlaying { get; }

        ReactiveCommand<Unit, Either<int, string>> UploadImage { get; }

        ReactiveCommand<Unit, string> DeleteImage { get; }

        ReactiveCommand<Unit, Either<int, string>> UploadAudio { get; }

        ReactiveCommand<Unit, string> DeleteAudio { get; }

        Interaction<string, bool> ConfirmDelete { get; }

        string Title { get; set; }

        string ImageUrl { get; }

        string AudioUrl { get; }
    }
}
