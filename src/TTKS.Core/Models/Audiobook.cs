namespace TTKS.Core.Models
{
    public class Audiobook : BaseEntity
    {
        public string Title { get; set; }

        public string ImageName { get; set; }

        public string ImageUrl { get; set; }

        public string AudioName { get; set; }

        public string AudioUrl { get; set; }
    }
}
