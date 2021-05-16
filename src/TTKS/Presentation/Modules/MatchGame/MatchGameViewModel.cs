using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Presentation.Modules
{
    public class MatchGameViewModel : BasePageViewModel, IMatchGameViewModel
    {
        public MatchGameViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Reading Program";
    }
}
