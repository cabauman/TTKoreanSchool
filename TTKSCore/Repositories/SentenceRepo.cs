using Firebase.Database;
using Firebase.Database.Offline;

namespace TTKSCore.Models
{
    public class SentenceRepo : FirebaseOfflineCacheRepo<ExampleSentence>
    {
        public SentenceRepo(
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
