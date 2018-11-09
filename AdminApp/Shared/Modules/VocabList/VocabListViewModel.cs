using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class VocabListViewModel : BasePageViewModel, IVocabListViewModel
    {
        public VocabListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Vocab";
    }
}
