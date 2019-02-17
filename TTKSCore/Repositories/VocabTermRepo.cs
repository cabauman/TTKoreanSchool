using Firebase.Database;
using Firebase.Database.Offline;

namespace TTKSCore.Models
{
    public class VocabTermRepo : FirebaseOfflineCacheRepo<VocabTerm>
    {
        public VocabTermRepo(
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
