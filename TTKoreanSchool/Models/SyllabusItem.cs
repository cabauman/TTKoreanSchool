using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class SyllabusItem : BaseEntity
    {
        [JsonProperty("agenda")]
        public string Agenda { get; set; }

        [JsonProperty("homework")]
        public string Homework { get; set; }

        [JsonProperty("fromTime")]
        public string FromTime { get; set; }

        [JsonProperty("toTime")]
        public string ToTime { get; set; }
    }
}