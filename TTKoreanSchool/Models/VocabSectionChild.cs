using System.Collections.Generic;

namespace TTKoreanSchool.Models
{
    public class VocabSectionChild
    {
        public VocabSectionChild(string id, string title, string iconId, bool isSubsection)
        {
            Id = id;
            Title = title;
            IconId = iconId;
            IsSubsection = isSubsection;
        }

        public string Id { get; }

        public string Title { get; }

        public string IconId { get; }

        public bool IsSubsection { get; }
    }
}