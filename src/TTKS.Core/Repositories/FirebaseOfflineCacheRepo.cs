using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DynamicData;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;

namespace TTKS.Core.Models
{
    public class FirebaseOfflineCacheRepo<T>
        where T : BaseEntity
    {
        private readonly RealtimeDatabase<T> _realtimeDb;
        private readonly ChildQuery _baseQuery;
        private readonly ISourceCache<T, string> _cache;
        private readonly ReadOnlyObservableCollection<T> _readOnlyCollection;
        //private readonly IObservable<IChangeSet<T, string>> _changeSet;

        private bool _cachePopulatedWithLocalItems;

        public FirebaseOfflineCacheRepo(
            FirebaseClient client,
            string path,
            string key = "",
            StreamingOptions streaming = StreamingOptions.None,
            InitialPullStrategy initialPull = InitialPullStrategy.Everything)
        {
            // The offline database filename is named after type T.
            // So, if you have more than one list of type T objects, you need to differentiate it
            // by adding a filename modifier; which is what we're using the "key" parameter for.
            _baseQuery = client.Child(path);
            _realtimeDb = _baseQuery
                .AsRealtimeDatabase<T>(key, string.Empty, streaming, initialPull, true);

            SyncExceptionThrown = Observable
                .FromEventPattern<ExceptionEventArgs>(
                    h => _realtimeDb.SyncExceptionThrown += h,
                    h => _realtimeDb.SyncExceptionThrown -= h);

            _cache = new SourceCache<T, string>(x => x.Id);
            _cache
                .Connect()
                .Bind(out _readOnlyCollection)
                .SubscribeSafe();
        }

        public IObservable<EventPattern<ExceptionEventArgs>> SyncExceptionThrown { get; }

        public IObservable<Unit> Add(T item)
        {
            return Observable
                .Start(() => item.Id = _realtimeDb.Post(item))
                .Do(_ => _cache.AddOrUpdate(item))
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> Add(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id = FirebaseKeyGenerator.Next()))
                .ToObservable()
                .SelectMany(Pull())
                .Do(_ => _cache.AddOrUpdate(items));
        }

        public IObservable<Unit> Upsert(T item)
        {
            return Observable
                .Start(() => _realtimeDb.Patch(item.Id, item))
                .Do(_ => _cache.AddOrUpdate(item));
        }

        public IObservable<Unit> Upsert(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id = x.Id ?? FirebaseKeyGenerator.Next()))
                .ToObservable()
                .SelectMany(_ => Pull())
                .Do(_ => _cache.AddOrUpdate(items));
        }

        public IObservable<Unit> Delete(string id)
        {
            return Observable
                .Start(() => _realtimeDb.Delete(id))
                .Do(_ => _cache.RemoveKey(id));
        }

        public IObservable<Unit> Delete(IEnumerable<T> items)
        {
            // Doesn't work offline. Need offline solution.
            return _baseQuery
                .PatchAsync(items.ToDictionary(x => x.Id, x => default(string)))
                .ToObservable()
                .SelectMany(_ => Pull())
                .Do(_ => _cache.Remove(items));
        }

        public IObservable<T> GetItem(string id)
        {
            return Observable
                .Start(
                    () =>
                    {
                        _realtimeDb.Database.TryGetValue(id, out OfflineEntry item);
                        return item?.Deserialize<T>();
                    });
        }

        public IObservable<ReadOnlyObservableCollection<T>> GetItems(bool fetchOnline = false)
        {
            return Observable
                .Start(
                    () =>
                    {
                        if (_cachePopulatedWithLocalItems && !fetchOnline)
                        {
                            return Observable.Return(Unit.Default);
                        }
                        else
                        {
                            return Pull()
                                .SelectMany(_ => _realtimeDb.Once())
                                .Do(MapKeyToId)
                                .Select(x => x.Object)
                                .ToList()
                                .Do(PopulateCache)
                                .Select(_ => Unit.Default);
                        }
                    })
                .SelectMany(x => x)
                .Select(_ => _readOnlyCollection);
        }

        private IObservable<Unit> Pull()
        {
            return _realtimeDb
                .PullAsync()
                .ToObservable();
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            firebaseObj.Object.Id = firebaseObj.Key;
        }

        private void PopulateCache(IList<T> items)
        {
            _cache.AddOrUpdate(items);
            _cachePopulatedWithLocalItems = true;
        }
    }
}
