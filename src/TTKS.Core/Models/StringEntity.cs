using Newtonsoft.Json;

namespace TTKS.Core.Models
{
    public class StringEntity : BaseEntity
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}