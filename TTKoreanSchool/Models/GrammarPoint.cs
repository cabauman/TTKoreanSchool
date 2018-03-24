using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class GrammarPoint : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("translation")]
        public string Translation { get; set; }

        [JsonProperty("extraInfoId")]
        public string ExtraInfoId { get; set; }

        [JsonProperty("sentenceIds")]
        public string[] SentenceIds { get; set; }
    }
}