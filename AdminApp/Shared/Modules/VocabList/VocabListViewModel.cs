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
    public class VocabListViewModel : BasePageViewModel, IVocabListViewModel
    {
        public VocabListViewModel(
            IViewStackService viewStackService = null,
            IRepository<VocabTerm> vocabTermRepository = null)
                : base(viewStackService)
        {
            VocabTermRepository = Locator.Current.GetService<IRepository<VocabTerm>>();
        }

        public override string Title => "Vocab";

        public ReactiveCommand<Unit, Unit> AddItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> UpdateItem { get; }

        public IRepository<VocabTerm> VocabTermRepository { get; }

        public ObservableCollection<IVocabItemViewModel> VocabItems { get; }
    }
}
