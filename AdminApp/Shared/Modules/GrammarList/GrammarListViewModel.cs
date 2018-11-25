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
    public class GrammarListViewModel : BasePageViewModel, IGrammarListViewModel
    {
        private IGrammarItemViewModel _selectedItem;

        public GrammarListViewModel(
            IRepository<GrammarPrinciple> grammarRepo = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            GrammarRepo = grammarRepo ?? Locator.Current.GetService<IRepository<GrammarPrinciple>>();
        }

        public override string Title => "Grammar";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<GrammarPrinciple> GrammarRepo { get; }

        public ObservableCollection<IGrammarItemViewModel> Items { get; }

        public IGrammarItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}
