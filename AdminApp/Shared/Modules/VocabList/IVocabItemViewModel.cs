using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IVocabItemViewModel
    {
        VocabTerm Model { get; }

        string Ko { get; set; }

        string Romanization { get; set; }

        string Translation { get; set; }

        string ImageIds { get; set; }

        string SentenceIds { get; set; }
    }
}
