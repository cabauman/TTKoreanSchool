using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKS.Core.Common;

namespace TTKS.Admin.Modules
{
    public class AudiobookViewModel : BasePageViewModel, IAudiobookViewModel
    {
        public AudiobookViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Audiobook";
    }
}
