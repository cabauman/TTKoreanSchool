using Firebase.Database;
using Firebase.Database.Offline;
using TTKS.Core.Models;

namespace TTKS.Data.Repositories
{
    public class VocabTermRepository : FirebaseOfflineCacheRepo<VocabTerm>, IVocabTermRepository
    {
        public VocabTermRepository(
            FirebaseClient client,
            string path,
            string key = "",
            StreamingOptions streaming = StreamingOptions.None,
            InitialPullStrategy initialPull = InitialPullStrategy.Everything)
                : base(client, path, key, streaming, initialPull)
        {
        }
    }
}
