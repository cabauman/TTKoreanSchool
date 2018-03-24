using System.Collections.Generic;
using Newtonsoft.Json;

namespace TTKoreanSchool.Models
{
    public class User : BaseEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("skypeId")]
        public string SkypeId { get; set; }

        [JsonProperty("phoneNum")]
        public string PhoneNum { get; set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; }

        [JsonProperty("instanceIdToken")]
        public string InstanceIdToken { get; set; }

        [JsonProperty("studyPoints")]
        public int StudyPoints { get; set; }

        [JsonProperty("didPayTuition")]
        public bool DidPayTuition { get; set; }

        [JsonProperty("didRegister")]
        public bool DidRegister { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        // Maybe should move under an admin node.
        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("courseIdToSemesterDict")]
        public IDictionary<string, string> CourseIdToSemesterDict { get; set; }
    }
}