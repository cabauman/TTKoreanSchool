using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class GrammarPrinciple : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("translation")]
        public string Translation { get; set; }

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("validConjugations")]
        public string ValidConjugations { get; set; }

        [JsonProperty("validLastWordConjugations")]
        public string ValidLastWordConjugations { get; set; }

        [JsonProperty("sentenceIds")]
        public string[] SentenceIds { get; set; }
    }
}