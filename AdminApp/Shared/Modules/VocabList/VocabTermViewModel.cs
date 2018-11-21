using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class VocabTermViewModel : BasePageViewModel, IVocabTermViewModel
    {
        public VocabTermViewModel(
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
        }

        public override string Title => "Vocab Term";

        public VocabTerm Model { get; }

        public string Ko { get; }

        public string Romanization { get; }

        public string Translation { get; }

        public string ImageUrl { get; }

        public string SentenceIds { get; }
    }
}
