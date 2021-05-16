namespace TTKS.Core.Models
{
    public class VocabTerm : BaseEntity
    {
        public string Ko { get; set; }

        public string WordClass { get; set; }

        public string HomonymSpecifier { get; set; }

        // Verbs
        public string Transitivity { get; set; }

        public string HonorificForm { get; set; }

        // Verbs
        public string PassiveForm { get; set; }

        // Adjectives
        public string AdverbForm { get; set; }

        public int AudioVersion { get; set; }

        public string ImageIds { get; set; }

        public string SentenceIds { get; set; }

        public string Notes { get; set; }

        public string Translation { get; set; }

        public string IsStarred { get; set; }
    }
}