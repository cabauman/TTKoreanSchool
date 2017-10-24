using System.Collections.Generic;

namespace TTKoreanSchool.Models
{
    public class VocabSection
    {
        public VocabSection(string id, string title, string colorTheme, IReadOnlyList<VocabSectionChild> children)
        {
            Id = id;
            Title = title;
            ColorTheme = colorTheme;
            Children = children;
        }

        public string Id { get; }

        public string Title { get; }

        public string ColorTheme { get; }

        public int Order { get; }

        public IReadOnlyList<VocabSectionChild> Children { get; }
    }
}