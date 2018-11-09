using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class VocabImageListViewModel : BasePageViewModel, IVocabImageListViewModel
    {
        public VocabImageListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Sentences";
    }
}
