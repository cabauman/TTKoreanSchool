using ReactiveUI;

namespace TongTongAdmin.Modules
{
    public interface IGrammarItemViewModel
    {
        string Ko { get; set; }

        string En { get; set; }

        string[] SentenceIds { get; set; }
    }
}
