using Newtonsoft.Json;

namespace TTKS.Core.Dtos
{
    public class ExampleSentenceDto : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }

        [JsonIgnore]
        public string Translation { get; set; }
    }
}