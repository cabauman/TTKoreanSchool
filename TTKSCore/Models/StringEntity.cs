using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class StringEntity : BaseEntity
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}