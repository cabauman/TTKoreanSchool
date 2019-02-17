using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Admin.Modules
{
    public class GrammarPrincipleViewModel : BasePageViewModel, IGrammarPrincipleViewModel
    {
        public GrammarPrincipleViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Reading Program";
    }
}
