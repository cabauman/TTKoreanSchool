using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.Repository;
using ReactiveUI;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public interface ISentenceListViewModel
    {
        ReactiveCommand<Unit, Unit> LoadItems { get; }

        ReactiveCommand<Unit, Unit> CreateItem { get; }

        ReactiveCommand<Unit, Unit> DeleteItem { get; }

        ReactiveCommand<Unit, Unit> SaveItem { get; }

        Interaction<string, bool> ConfirmDelete { get; }

        ISentenceItemViewModel SelectedItem { get; set; }

        IRepository<ExampleSentence> SentenceRepo { get; }

        ObservableCollection<ISentenceItemViewModel> Items { get; }
    }
}
