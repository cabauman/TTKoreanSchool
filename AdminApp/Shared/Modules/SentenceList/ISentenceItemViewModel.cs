using ReactiveUI;

namespace TongTongAdmin.Modules
{
    public interface ISentenceItemViewModel
    {
        string Ko { get; set; }

        string Romanization { get; set; }

        string En { get; }
    }
}
