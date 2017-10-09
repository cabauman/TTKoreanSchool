using System.Collections.Generic;

namespace TTKoreanSchool.Models
{
    public class CourseRegistration
    {
        public string UserId { get; }

        public string CourseTitle { get; }

        public string DaysAvailable { get; }

        public string Location { get; }

        public string RegistrantName { get; }

        public Dictionary<string, int> TimeStampMap { get; }

        public bool IsConfirmed { get; set; }
    }
}
