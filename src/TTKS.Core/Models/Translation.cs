using Newtonsoft.Json;

namespace TTKS.Core.Models
{
    public class Translation : BaseEntity
    {
        [JsonProperty("value")]
        public string Value { get; set; } = string.Empty;
    }
}