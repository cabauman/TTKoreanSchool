using System.Collections.Generic;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IVocabItemViewModel
    {
        VocabTerm Model { get; }

        string Ko { get; set; }

        string Romanization { get; set; }

        string En { get; set; }

        IList<string> ImageIds { get; set; }

        IList<string> SentenceIds { get; set; }
    }
}
