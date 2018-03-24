using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class VocabSectionChild : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("iconId")]
        public string IconId { get; set; }
    }
}