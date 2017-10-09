using System.Collections.Generic;

namespace TTKoreanSchool.Models
{
    public class StudySet
    {
        public string Id { get; }

        public string Title { get; }

        public string IconId { get; }

        public List<Term> Terms { get; }
    }
}
