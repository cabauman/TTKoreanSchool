using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Presentation.Modules
{
    public class AboutUsViewModel : BasePageViewModel, IAboutUsViewModel
    {
        public AboutUsViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Home";
    }
}
