using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TongTongAdmin.Modules
{
    public class AudioBookViewModel : BasePageViewModel, IAudioBookViewModel
    {
        public AudioBookViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Reading Program";
    }
}
