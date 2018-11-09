using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TTKoreanSchool.Modules
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
