using Newtonsoft.Json;

namespace TTKS.Core.Models
{
    public class ExampleSentence : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }

        [JsonIgnore]
        public string Translation { get; set; }
    }
}