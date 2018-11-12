using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using Plugin.MediaManager.Abstractions.Implementations;
using Plugin.MediaManager.Reactive;
using ReactiveUI;
using Splat;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class AudiobookListViewModel : BasePageViewModel, IAudiobookListViewModel
    {
        private readonly ObservableAsPropertyHelper<int> _uploadProgress;

        private IAudiobookItemViewModel _selectedItem;

        public AudiobookListViewModel(
            IViewStackService viewStackService = null,
            IRepository<Audiobook> audioBookRepository = null)
                : base(viewStackService)
        {
            AudiobookRepository = Locator.Current.GetService<IRepository<Audiobook>>();
            AudiobookItems = new ObservableCollection<IAudiobookItemViewModel>();
            ConfirmDelete = new Interaction<string, bool>();

            CreateItem = ReactiveCommand.Create(
                () => AudiobookItems.Add(new AudiobookItemViewModel()));

            SaveItem = ReactiveCommand.CreateFromObservable(
                () => AudiobookRepository.Upsert(SelectedItem.Model));

            DeleteItem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ConfirmDelete
                        .Handle(SelectedItem.ImageUrl)
                        .Where(result => result)
                        .SelectMany(_ => AudiobookRepository.Delete(SelectedItem.Model.Id))
                        .Do(_ => AudiobookItems.Remove(SelectedItem));
                });

            _uploadProgress = Observable
                .Merge(UploadImage, UploadAudio)
                .Where(x => x.IsLeft)
                .Select(x => x.Left)
                .ToProperty(this, x => x.UploadProgress);

            AudiobookItems
                .ToObservable()
                .SelectMany(x => x.UploadImage)
                .Subscribe(x => { });

            AudiobookItems
                .ToObservable()
                .SelectMany(x => x.PlayAudio2)
                .Subscribe(x => MediaManager.Play(new MediaFile(x)));
        }

        public override string Title => "Audiobooks";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IObservable<long> ModifiedItemPulse { get; }

        public IRepository<Audiobook> AudiobookRepository { get; }

        public IFirebaseStorageService FirebaseStorageService { get; }

        public IAudiobookItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ObservableCollection<IAudiobookItemViewModel> AudiobookItems { get; }



        public int UploadProgress => _uploadProgress.Value;

        public ReactiveCommand<Unit, Unit> PlayAudio { get; }

        public ReactiveCommand<Unit, Unit> StopAudio { get; }

        public ReactiveMediaManager MediaManager { get; }
    }
}
