namespace TTKoreanSchool.Models
{
    public class ExampleSentence
    {
        public ExampleSentence(string id, string ko, string romanization, string translation)
        {
            Id = id;
            Ko = ko;
            Romanization = romanization;
            Translation = translation;
        }

        public string Id { get; }

        public string Ko { get; }

        public string Romanization { get; }

        public string Translation { get; }
    }
}