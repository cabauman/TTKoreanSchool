using ReactiveUI;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public interface ISentenceItemViewModel
    {
        ExampleSentence Model { get; }

        string Ko { get; set; }

        string Romanization { get; set; }

        string En { get; }
    }
}
