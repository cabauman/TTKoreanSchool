using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKoreanSchool.Common;

namespace TTKoreanSchool.Modules
{
    public class HangulHomeViewModel : BasePageViewModel, IHangulHomeViewModel
    {
        public HangulHomeViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Hangul";
    }
}
