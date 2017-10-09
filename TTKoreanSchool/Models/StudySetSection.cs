using System.Collections.Generic;

namespace TTKoreanSchool.Models
{
    public class StudySetSection
    {
        public string Id { get; }

        public string Title { get; }

        public string ColorTheme { get; }

        public int Order { get; }

        public List<StudySet> StudySets { get; }
    }
}
