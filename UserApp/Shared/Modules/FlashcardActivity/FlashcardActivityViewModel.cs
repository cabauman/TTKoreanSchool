using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TTKoreanSchool.Modules
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
