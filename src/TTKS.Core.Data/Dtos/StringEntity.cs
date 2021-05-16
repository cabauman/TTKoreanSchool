using Newtonsoft.Json;

namespace TTKS.Core.Dtos
{
    public class StringEntity : BaseEntity
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}