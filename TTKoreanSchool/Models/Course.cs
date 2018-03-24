using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class Course : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tuition")]
        public string Tuition { get; set; }

        [JsonProperty("hourlyRate")]
        public uint HourlyRate { get; set; }

        [JsonProperty("semester")]
        public string Semester { get; set; } // "yyyy-MM-dd HH:mm:ssZ"

        [JsonProperty("students")]
        public IDictionary<string, string> Students { get; set; }


        // Unsure about.
        [JsonProperty("thisSemester")]
        public bool ThisSemester { get; set; }

        [JsonProperty("dayToTimeMap")]
        public IDictionary<string, object> DayToTimeMap { get; set; }

        [JsonProperty("courseType")]
        public string CourseType { get; set; }

        [JsonProperty("uidToAmountPaidDict")]
        public IDictionary<string, uint> UidToAmountPaidDict { get; set; }
    }
}