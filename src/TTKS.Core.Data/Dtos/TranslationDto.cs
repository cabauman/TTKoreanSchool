using Newtonsoft.Json;

namespace TTKS.Core.Dtos
{
    public class TranslationDto : BaseEntity
    {
        [JsonProperty("value")]
        public string Value { get; set; } = string.Empty;
    }
}