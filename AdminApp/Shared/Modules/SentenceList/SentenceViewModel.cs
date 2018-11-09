using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class SentenceViewModel : BasePageViewModel, ISentenceViewModel
    {
        public SentenceViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Reading Program";
    }
}
