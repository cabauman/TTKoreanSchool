using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public class VocabImageListViewModel : BasePageViewModel, IVocabImageListViewModel
    {
        private IVocabImageItemViewModel _selectedItem;

        public VocabImageListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Vocab Images";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<VocabImage> VocabImageRepo { get; }

        public ObservableCollection<IVocabImageItemViewModel> Items { get; }

        public IVocabImageItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}
