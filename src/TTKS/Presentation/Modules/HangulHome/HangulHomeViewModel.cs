using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Presentation.Modules
{
    public class HangulHomeViewModel : BasePageViewModel, IHangulHomeViewModel
    {
        public HangulHomeViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => Resx.AppResources.Hangul;
    }
}
