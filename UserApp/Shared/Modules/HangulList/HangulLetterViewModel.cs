using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKoreanSchool.Common;

namespace TTKoreanSchool.Modules
{
    public class HangulLetterViewModel : BasePageViewModel, IHangulListViewModel
    {
        public HangulLetterViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Hangul List";
    }
}
