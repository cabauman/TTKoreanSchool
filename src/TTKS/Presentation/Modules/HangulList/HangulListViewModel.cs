using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Presentation.Modules
{
    public class HangulListViewModel : BasePageViewModel, IHangulListViewModel
    {
        public HangulListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Hangul List";
    }
}
