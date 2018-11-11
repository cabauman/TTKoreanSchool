using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class VocabImage : BaseEntity
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("tags")]
        public IList<string> Tags { get; set; }
    }
}
