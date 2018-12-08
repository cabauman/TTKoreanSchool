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

        public string Ko { get; set; }

        public string En { get; set; }

        public string Romanization { get; }

        public string HomonymSpecifier { get; set; }

        public string WordClass { get; set; }

        public string Transitivity { get; set; }

        public string HonorificForm { get; set; }

        public string PassiveForm { get; set; }

        public string AdverbForm { get; set; }

        public string Notes { get; set; }

        public IList<string> ImageUrls { get; set; }

        public IList<string> Sentences { get; set; }
    }
}
