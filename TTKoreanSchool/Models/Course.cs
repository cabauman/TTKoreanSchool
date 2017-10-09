namespace TTKoreanSchool.Models
{
    public class Course
    {
        public string Id { get; }

        public string Title { get; set; }

        public string Tuition { get; set; }

        public bool ThisSemester { get; set; }
    }
}
