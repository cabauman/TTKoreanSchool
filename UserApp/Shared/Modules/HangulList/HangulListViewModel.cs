using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TTKoreanSchool.Modules
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
