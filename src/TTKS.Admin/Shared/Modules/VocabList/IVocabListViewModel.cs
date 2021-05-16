using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
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

        VocabTermRepository VocabTermRepo { get; }

        ObservableCollection<IVocabItemViewModel> Items { get; }

        ReadOnlyObservableCollection<StringEntity> Homonyms { get; }
    }
}
