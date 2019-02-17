using Newtonsoft.Json;

namespace TTKS.Core.Models
{
    public class Audiobook : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("imageName")]
        public string ImageName { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("audioName")]
        public string AudioName { get; set; }

        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }
    }
}
