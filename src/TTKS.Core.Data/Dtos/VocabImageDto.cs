using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKS.Core.Dtos
{
    public class VocabImageDto : BaseEntity
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("tags")]
        public IList<string> Tags { get; set; }
    }
}
