namespace TTKS.Core.Models
{
    public class GrammarPrinciple : BaseEntity
    {
        public string Ko { get; set; }

        public string Translation { get; set; }

        public string Difficulty { get; set; }

        public string ValidConjugations { get; set; }

        public string ValidLastWordConjugations { get; set; }

        public string[] SentenceIds { get; set; }
    }
}