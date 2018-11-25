using System.Collections.Generic;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IVocabItemViewModel
    {
        VocabTerm Model { get; }

        string Ko { get; set; }

        string En { get; set; }

        string HomonymSpecifier { get; set; }

        string WordClass { get; set; }

        string Transitivity { get; set; }

        string HonorificForm { get; set; }

        string PassiveForm { get; set; }

        string AdverbForm { get; set; }

        string Notes { get; set; }

        IList<string> ImageIds { get; set; }

        IList<string> SentenceIds { get; set; }
    }
}
