using System;
using ReactiveUI;

namespace TTKS.Admin.Modules
{
    public class HangulItemViewModel : ReactiveObject, IHangulItemViewModel
    {
        public HangulItemViewModel()
        {
        }

        public string Ko { get; set; }

        public string Romanization { get; set; }

        public string Char { get; set; }

        public bool IsVowel { get; set; }

        public bool IsComposite { get; set; }

        public string SimilarSound { get; set; }
    }
}
