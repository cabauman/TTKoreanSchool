using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.Repository;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IVocabListViewModel
    {
        ReactiveCommand<Unit, Unit> LoadItems { get; }

        ReactiveCommand<Unit, Unit> CreateItem { get; }

        ReactiveCommand<Unit, Unit> DeleteItem { get; }

        ReactiveCommand<Unit, Unit> SaveItem { get; }

        Interaction<string, bool> ConfirmDelete { get; }

        IVocabItemViewModel SelectedItem { get; set; }

        IRepository<VocabTerm> VocabTermRepo { get; }

        ObservableCollection<IVocabItemViewModel> Items { get; }
    }
}
