using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class HangulLetter : BaseEntity
    {
        [JsonProperty("ko")]
        public string Ko { get; set; }

        [JsonProperty("romanization")]
        public string Romanization { get; set; }

        [JsonProperty("char")]
        public string Char { get; set; }

        [JsonProperty("isVowel")]
        public bool IsVowel { get; set; }

        [JsonProperty("isComposite")]
        public bool IsComposite { get; set; }

        [JsonProperty("similarSound")]
        public string SimilarSound { get; set; }

        [JsonProperty("soundToSampleWordsDict")]
        public IDictionary<string, string> SoundToSampleWordsDict { get; set; }
    }
}