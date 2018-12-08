using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Acr.UserDialogs;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using NSubstitute;
using ReactiveUI;
using TongTongAdmin.Modules;
using TTKSCore.Models;

namespace TTKoreanSchool.Tests.AdminApp
{
    public class AudiobookListViewModelBuilder : IBuilder
    {
        private int _itemCount;
        private bool _activate;
        private bool? _confirmDeleteInteraction;
        private IViewStackService _viewStackService;
        private IRepository<Audiobook> _audiobookRepo;
        private IFirebaseStorageService _firebaseStorageService;
        private IUserDialogs _dialogService;
        private IScheduler _mainScheduler;

        public AudiobookListViewModelBuilder()
        {
            _activate = true;
            _viewStackService = Substitute.For<IViewStackService>();
            _audiobookRepo = Substitute.For<IRepository<Audiobook>>();
            _firebaseStorageService = Substitute.For<IFirebaseStorageService>();
            _dialogService = Substitute.For<IUserDialogs>();
            _mainScheduler = CurrentThreadScheduler.Instance;
        }

        public AudiobookListViewModelBuilder WithActivation(bool activate = true) =>
            this.With(ref _activate, activate);

        public AudiobookListViewModelBuilder WithFirebaseStorageService(IFirebaseStorageService storageService) =>
            this.With(ref _firebaseStorageService, storageService);

        public AudiobookListViewModelBuilder WithFirebaseStorageServiceMock(out IFirebaseStorageService storageService) =>
            this.WithFirebaseStorageService(storageService = Substitute.For<IFirebaseStorageService>());

        public AudiobookListViewModelBuilder WithAudiobookRepo(IRepository<Audiobook> audiobookRepo) =>
            this.With(ref _audiobookRepo, audiobookRepo);

        public AudiobookListViewModelBuilder WithAudiobookRepoMock(out IRepository<Audiobook> audiobookRepo) =>
            this.WithAudiobookRepo(audiobookRepo = Substitute.For<IRepository<Audiobook>>());

        public AudiobookListViewModelBuilder WithDialogService(IUserDialogs dialogService) =>
            this.With(ref _dialogService, dialogService);

        public AudiobookListViewModelBuilder WithDialogServiceMock(out IUserDialogs dialogService) =>
            this.WithDialogService(dialogService = Substitute.For<IUserDialogs>());

        public AudiobookListViewModelBuilder WithConfirmDeleteInteraction(bool confirmDeleteInteraction) =>
            this.With(ref _confirmDeleteInteraction, confirmDeleteInteraction);

        public AudiobookListViewModelBuilder WithItemCount(int itemCount) =>
            this.With(ref _itemCount, itemCount);

        public AudiobookListViewModelBuilder WithMainScheduler(IScheduler scheduler) =>
            this.With(ref _mainScheduler, scheduler);

        public IAudiobookListViewModel Build()
        {
            var result = new AudiobookListViewModel(
                _audiobookRepo,
                _firebaseStorageService,
                _dialogService,
                _mainScheduler,
                _viewStackService);

            if (_activate)
            {
                result
                    .Activator
                    .Activate();

                IEnumerable<Audiobook> audiobooks = Enumerable.Repeat(new Audiobook(), _itemCount);
                _audiobookRepo.GetItems().Returns(Observable.Return(audiobooks));
                result.LoadItems.Execute().Subscribe();
            }

            if (_confirmDeleteInteraction.HasValue)
            {
                result
                    .ConfirmDelete
                    .RegisterHandler(context => context.SetOutput(_confirmDeleteInteraction.Value));
            }

            return result;
        }
    }
}
