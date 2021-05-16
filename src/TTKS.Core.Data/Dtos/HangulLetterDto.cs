using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKS.Core.Dtos
{
    public class HangulLetterDto : BaseEntity
    {
        [JsonProperty("soundToSampleWordsDict")]
        public IDictionary<string, string> SoundToSampleWordsDict { get; set; }
    }
}