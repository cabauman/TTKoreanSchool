namespace TTKoreanSchool.Models
{
    public class Term
    {
        public Term() { }

        public Term(
            string id,
            string ko,
            string romanization,
            string translation,
            string extraInfoId,
            string[] imageIds,
            string[] sentenceIds)
        {
            Id = id;
            Ko = ko;
            Romanization = romanization;
            Translation = translation;
            ExtraInfoId = extraInfoId;
            ImageIds = imageIds;
            SentenceIds = sentenceIds;
        }

        public string Id { get; }

        public string Ko { get; }

        public string Romanization { get; }

        public string Translation { get; }

        public string ExtraInfoId { get; }

        public string[] ImageIds { get; }

        public string[] SentenceIds { get; }
    }
}
