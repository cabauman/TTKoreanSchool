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
            int audioVersion,
            string[] imageIds,
            string[] sentenceIds)
        {
            Id = id;
            Ko = ko;
            Romanization = romanization;
            Translation = translation;
            ExtraInfoId = extraInfoId;
            AudioVersion = audioVersion;
            ImageIds = imageIds;
            SentenceIds = sentenceIds;
        }

        public string Id { get; }

        public string Ko { get; }

        public string Romanization { get; }

        public string Translation { get; }

        public string ExtraInfoId { get; }

        public int AudioVersion { get; }

        public string[] ImageIds { get; }

        public string[] SentenceIds { get; }
    }
}
