using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class ExampleSentence : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("romanization")]
        public string Romanization { get; set; }

        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }

        [JsonIgnore]
        public string Translation { get; set; }
    }
}