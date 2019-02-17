using System;
using ReactiveUI;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public class SentenceItemViewModel : ReactiveObject, ISentenceItemViewModel
    {
        public SentenceItemViewModel(ExampleSentence model)
        {
        }

        public ExampleSentence Model { get; }

        public string Ko { get; set; }

        public string Romanization { get; set; }

        public string En { get; }
    }
}
