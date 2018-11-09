using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class GrammarListViewModel : BasePageViewModel, IGrammarListViewModel
    {
        public GrammarListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Grammar";
    }
}
