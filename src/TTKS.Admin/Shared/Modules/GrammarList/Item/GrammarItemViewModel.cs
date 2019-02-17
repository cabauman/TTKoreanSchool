using System;
using System.Collections.Generic;
using ReactiveUI;

namespace TTKS.Admin.Modules
{
    public class GrammarItemViewModel : ReactiveObject, IGrammarItemViewModel
    {
        public GrammarItemViewModel()
        {
        }

        public string Ko { get; set; }

        public string En { get; set; }

        public IList<string> SentenceIds { get; set; }
    }
}
