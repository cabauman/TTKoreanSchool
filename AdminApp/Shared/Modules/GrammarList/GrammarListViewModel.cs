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
        public GrammarListViewModel(
            IViewStackService viewStackService = null,
            IRepository<GrammarPrinciple> grammarPrincipleRepository = null)
                : base(viewStackService)
        {
            GrammarPrincipleRepository = Locator.Current.GetService<IRepository<GrammarPrinciple>>();
        }

        public override string Title => "Grammar";

        public ReactiveCommand<Unit, Unit> AddItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> UpdateItem { get; }

        public IRepository<GrammarPrinciple> GrammarPrincipleRepository { get; }

        public ObservableCollection<IGrammarItemViewModel> GrammarItems { get; }
    }
}
