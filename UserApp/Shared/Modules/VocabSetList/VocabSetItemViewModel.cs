using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TTKoreanSchool.Modules
{
    public class VocabSetItemViewModel : BasePageViewModel, IVocabSetItemViewModel
    {
        public VocabSetItemViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Vocab";
    }
}
