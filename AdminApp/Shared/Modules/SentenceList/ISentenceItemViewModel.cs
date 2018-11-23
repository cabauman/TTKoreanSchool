using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface ISentenceItemViewModel
    {
        ExampleSentence Model { get; }

        string Ko { get; set; }

        string Romanization { get; set; }

        string En { get; }
    }
}
