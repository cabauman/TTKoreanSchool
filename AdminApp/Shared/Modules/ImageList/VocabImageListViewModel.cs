using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class VocabImageListViewModel : BasePageViewModel, IVocabImageListViewModel
    {
        public VocabImageListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Vocab Images";

        public ReactiveCommand<Unit, Unit> AddItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> UpdateItem { get; }

        public ObservableCollection<IVocabImageItemViewModel> VocabImageItems { get; }
    }
}
