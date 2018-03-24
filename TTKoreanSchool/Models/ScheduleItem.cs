using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class ScheduleItem : BaseEntity
    {
        [JsonProperty("teachers")]
        public string Teachers { get; set; }

        [JsonProperty("days")]
        public string Days { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("dayKeys")]
        public List<string> DayKeys { get; set; } // 0: d1 (Sunday)  -  1: d7 (Saturday)

        [JsonProperty("fromTime")]
        public string FromTime { get; set; } // 11:00 AM

        [JsonProperty("toTime")]
        public string ToTime { get; set; } // 11:00 AM
    }
}