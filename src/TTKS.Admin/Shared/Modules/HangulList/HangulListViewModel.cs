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
    public class HangulListViewModel : BasePageViewModel, IHangulListViewModel
    {
        public HangulListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Hangul Letters";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IHangulItemViewModel SelectedItem { get; set; }

        public IRepository<HangulLetter> HangulLetterRepo { get; }

        public ObservableCollection<IHangulItemViewModel> Items { get; }
    }
}
