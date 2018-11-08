using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKoreanSchool.Common;

namespace TTKoreanSchool.Modules
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
