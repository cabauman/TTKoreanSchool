using Firebase.Database;
using Firebase.Database.Offline;
using TTKS.Core.Models;

namespace TTKS.Admin.Repositories
{
    public class HomonymRepository : FirebaseOfflineCacheRepo<StringEntity>, IHomonymRepository
    {
        public HomonymRepository(
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
