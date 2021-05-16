using Newtonsoft.Json;

namespace TTKS.Core.Dtos
{
    public class VocabTermDto : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("wordClass")]
        public string WordClass { get; set; }

        [JsonProperty("homonymSpecifier")]
        public string HomonymSpecifier { get; set; }

        // Verbs
        [JsonProperty("transitivity")]
        public string Transitivity { get; set; }

        [JsonProperty("honorificForm")]
        public string HonorificForm { get; set; }

        // Verbs
        [JsonProperty("passiveForm")]
        public string PassiveForm { get; set; }

        // Adjectives
        [JsonProperty("adverbForm")]
        public string AdverbForm { get; set; }

        [JsonProperty("audioVersion")]
        public int AudioVersion { get; set; }

        [JsonProperty("imageIds")]
        public string ImageIds { get; set; }

        [JsonProperty("sentenceIds")]
        public string SentenceIds { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}