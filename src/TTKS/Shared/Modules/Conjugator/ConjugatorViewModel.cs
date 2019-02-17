using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Modules
{
    public class ConjugatorViewModel : BasePageViewModel, IConjugatorViewModel
    {
        public ConjugatorViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Conjugator";
    }
}
