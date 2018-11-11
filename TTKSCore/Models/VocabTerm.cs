using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class VocabTerm : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("romanization")]
        public string Romanization { get; set; }

        [JsonProperty("extraInfoId")]
        public string ExtraInfoId { get; set; }

        [JsonProperty("audioVersion")]
        public int AudioVersion { get; set; }

        [JsonProperty("imageIds")]
        public string ImageIds { get; set; }

        [JsonProperty("sentenceIds")]
        public string SentenceIds { get; set; }

        [JsonIgnore]
        public string Translation { get; set; }

        [JsonIgnore]
        public string IsStarred { get; set; }
    }
}