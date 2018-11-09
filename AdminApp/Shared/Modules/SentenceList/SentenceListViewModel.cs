using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class SentenceListViewModel : BasePageViewModel, ISentenceListViewModel
    {
        public SentenceListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Sentences";
    }
}
