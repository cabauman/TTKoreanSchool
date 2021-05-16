using System.Collections.Generic;

namespace TTKS.Core.Models
{
    public class VocabImage : BaseEntity
    {
        public string Url { get; set; }

        public IList<string> Tags { get; set; }
    }
}
