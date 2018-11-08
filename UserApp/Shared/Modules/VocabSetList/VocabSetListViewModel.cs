using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKoreanSchool.Common;

namespace TTKoreanSchool.Modules
{
    public class VocabSetListViewModel : BasePageViewModel, IVocabSetListViewModel
    {
        public VocabSetListViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Vocab";
    }
}
