using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class BaseEntity
    {
        [JsonIgnore]
        public string Id { get; set; }
    }
}