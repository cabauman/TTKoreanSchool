using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.Repository;
using ReactiveUI;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public interface IHangulListViewModel
    {
        ReactiveCommand<Unit, Unit> LoadItems { get; }

        ReactiveCommand<Unit, Unit> CreateItem { get; }

        ReactiveCommand<Unit, Unit> DeleteItem { get; }

        ReactiveCommand<Unit, Unit> SaveItem { get; }

        Interaction<string, bool> ConfirmDelete { get; }

        IHangulItemViewModel SelectedItem { get; set; }

        IRepository<HangulLetter> HangulLetterRepo { get; }

        ObservableCollection<IHangulItemViewModel> Items { get; }
    }
}
