using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class VocabSection : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("colorTheme")]
        public string ColorTheme { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("study-sets")]
        public IDictionary<string, VocabSectionChild> StudySets { get; set; }

        [JsonProperty("subsections")]
        public IDictionary<string, VocabSectionChild> Subsections { get; set; }
    }
}