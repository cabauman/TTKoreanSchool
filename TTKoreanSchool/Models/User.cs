namespace TTKoreanSchool.Models
{
    public class User
    {
        public string Id { get; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNum { get; set; }

        public string PhotoUrl { get; set; }

        public string InstanceIdToken { get; set; }

        public int StudyPoints { get; set; }
    }
}
