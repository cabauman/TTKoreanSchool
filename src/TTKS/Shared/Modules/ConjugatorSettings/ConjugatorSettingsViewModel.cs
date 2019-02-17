using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Modules
{
    public class ConjugatorSettingsViewModel : BasePageViewModel, IConjugatorSettingsViewModel
    {
        public ConjugatorSettingsViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Conjugator Settings";
    }
}
