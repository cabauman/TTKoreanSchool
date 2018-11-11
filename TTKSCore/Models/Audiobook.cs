using Newtonsoft.Json;

namespace TTKSCore.Models
{
    public class Audiobook : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }
    }
}
