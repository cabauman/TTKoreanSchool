using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class Translation : BaseEntity
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}