using System.Collections.Generic;

namespace TTKS.Core.Models
{
    public class HangulLetter : BaseEntity
    {
        public string Ko { get; set; }

        public string Romanization { get; set; } // => Romanizer.Romanize();

        public string Char { get; set; }

        public bool IsVowel { get; set; }

        public bool IsComposite { get; set; }

        public string SimilarSound { get; set; }

        public IDictionary<string, string> SoundToSampleWordsDict { get; set; }
    }
}