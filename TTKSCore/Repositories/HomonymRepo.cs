using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DynamicData;
using Firebase.Database;
using Firebase.Database.Offline;
using GameCtor.FirebaseDatabase.DotNet;

namespace TTKSCore.Models
{
    public class HomonymRepo : FirebaseOfflineRepo<StringEntity>
    {
        private ISourceCache<StringEntity, string> _cache;
        private IObservable<IChangeSet<StringEntity, string>> _changeSet;

        public HomonymRepo(
            FirebaseClient client,
            string path,
            string key = "",
            StreamingOptions streaming = StreamingOptions.None,
            InitialPullStrategy initialPull = InitialPullStrategy.Everything)
                : base(client, path, key, streaming, initialPull)
        {
        }

        public IObservable<IChangeSet<StringEntity, string>> ChangeSet { get; }

        public IObservable<IEnumerable<StringEntity>> GetCachedItems()
        {
            if (_cache == null)
            {
                _cache = new SourceCache<StringEntity, string>(x => x.Id);
                _changeSet = _cache.Connect();

                return GetItems()
                    .Do(x => _cache.AddOrUpdate(x));
            }

            return Observable.Return(_cache.Items);
        }

        public IObservable<IChangeSet<StringEntity, string>> GetChangeSet()
        {
            return GetItems()
                .SelectMany(_ => _changeSet);
        }
    }
}
