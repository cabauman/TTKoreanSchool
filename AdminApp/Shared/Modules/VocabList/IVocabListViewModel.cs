using System.Collections.Generic;
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

        ReactiveCommand<Unit, Unit> SaveAllModifiedItems { get; }

        Interaction<string, bool> ConfirmDelete { get; }

        IVocabItemViewModel SelectedItem { get; set; }

        IRepository<VocabTerm> VocabTermRepo { get; }

        ObservableCollection<IVocabItemViewModel> Items { get; }

        List<StringEntity> Homonyms { get; }
    }
}
