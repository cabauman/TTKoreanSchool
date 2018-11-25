using System;
using System.Collections.Generic;

namespace TTKSCore.Common
{
    public static class StudyContentConfig
    {
        public static readonly List<string> WordClasses = new List<string>
        {
            "Verb",
            "Noun",
            "Adjective",
            "Adverb",
            "Pronoun",
            "Numeral",
            "Interjection",
            "Particle",
            "Determiner",
            "Hangul",
            "Phrase",
            "Grammar",
        };

        public static readonly List<string> Transitivity = new List<string>
        {
            "Transitive",
            "Intransitive",
            "Both",
        };
    }
}
