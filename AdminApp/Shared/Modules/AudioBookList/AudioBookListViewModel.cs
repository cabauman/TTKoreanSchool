using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class AudiobookListViewModel : BasePageViewModel, IAudiobookListViewModel
    {
        private IAudiobookItemViewModel _selectedItem;

        public AudiobookListViewModel(
            IViewStackService viewStackService = null,
            IRepository<Audiobook> audioBookRepository = null)
                : base(viewStackService)
        {
            AudiobookRepository = Locator.Current.GetService<IRepository<Audiobook>>();
            AudiobookItems = new ObservableCollection<IAudiobookItemViewModel>();

            CreateItem = ReactiveCommand.Create(
                () => AudiobookItems.Add(new AudiobookItemViewModel()));

            UpsertItem = ReactiveCommand.CreateFromObservable(
                () => AudiobookRepository.Upsert(SelectedItem.Model));

            DeleteItem = ReactiveCommand.CreateFromObservable(
                () => AudiobookRepository.Delete(SelectedItem.Model.Id));
        }

        public override string Title => "Audiobooks";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> UpsertItem { get; }

        public IObservable<long> ModifiedItemPulse { get; }

        public IRepository<Audiobook> AudiobookRepository { get; }

        public IAudiobookItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ObservableCollection<IAudiobookItemViewModel> AudiobookItems { get; }
    }
}
