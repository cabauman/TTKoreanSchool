namespace TTKoreanSchool.Models
{
    public class Term
    {
        public string Id { get; }

        public string Ko { get; }

        public string Romanization { get; }

        public string Translation { get; }

        public string ExtraInfoId { get; }

        public string[] ImageIds { get; }

        public string[] SentenceIds { get; }
    }
}
