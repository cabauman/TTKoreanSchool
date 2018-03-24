using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class ExampleSentence : BaseEntity
    {
        public string Ko { get; set; }

        public string Romanization { get; set; }

        [JsonIgnore]
        public string Translation { get; set; }
    }
}