using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Presentation.Modules
{
    public class FlashcardActivityViewModel : BasePageViewModel, IFlashcardActivityViewModel
    {
        public FlashcardActivityViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Hangul";
    }
}
