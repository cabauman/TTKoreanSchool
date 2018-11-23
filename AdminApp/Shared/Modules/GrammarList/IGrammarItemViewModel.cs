using System.Collections.Generic;
using ReactiveUI;

namespace TongTongAdmin.Modules
{
    public interface IGrammarItemViewModel
    {
        string Ko { get; set; }

        string En { get; set; }

        IList<string> SentenceIds { get; set; }
    }
}
