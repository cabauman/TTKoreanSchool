using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.Repository;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IGrammarListViewModel
    {
        ReactiveCommand<Unit, Unit> LoadItems { get; }

        ReactiveCommand<Unit, Unit> CreateItem { get; }

        ReactiveCommand<Unit, Unit> DeleteItem { get; }

        ReactiveCommand<Unit, Unit> SaveItem { get; }

        Interaction<string, bool> ConfirmDelete { get; }

        IGrammarItemViewModel SelectedItem { get; set; }

        IRepository<GrammarPrinciple> GrammarRepo { get; }

        ObservableCollection<IGrammarItemViewModel> Items { get; }
    }
}
