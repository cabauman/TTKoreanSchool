using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using ReactiveUI;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseRepo<T>
        where T : class
    {
        protected IObservable<T> ReadAll(ChildQuery childQuery, bool useCache = true)
        {
            var realtimeDb = childQuery
                .AsRealtimeDatabase<T>(string.Empty, string.Empty, StreamingOptions.LatestOnly, InitialPullStrategy.Everything, true);

            realtimeDb.SyncExceptionThrown += (s, ex) => Console.WriteLine(ex.Exception);

            if(!useCache || realtimeDb.Database?.Count == 0)
            {
                return realtimeDb
                    .PullAsync()
                    .ToObservable()
                    .SubscribeOn(RxApp.TaskpoolScheduler)
                    .SelectMany(_ => ReadAll(realtimeDb));
            }

            return ReadAll(realtimeDb);
        }

        protected IObservable<T> Read(ChildQuery childQuery, string key)
        {
            return childQuery
                .OnceSingleAsync<T>()
                .ToObservable()
                .Do(obj => MapKeyToId(obj, key));
        }

        protected IObservable<Unit> Add(ChildQuery childQuery, T obj)
        {
            return childQuery
                .PostAsync(obj)
                .ToObservable()
                .Do(MapKeyToId)
                .Select(x => Unit.Default);

            //return Observable
            //    .Start(
            //        () =>
            //        {
            //            childQuery
            //                .PostAsync(obj)
            //                .ToObservable()
            //                .Do(MapKeyToId);
            //        });
        }

        protected IObservable<Unit> Update(ChildQuery childQuery, T obj)
        {
            return childQuery
                .PutAsync(obj)
                .ToObservable();
        }

        protected IObservable<Unit> Delete(ChildQuery childQuery)
        {
            return childQuery
                .DeleteAsync()
                .ToObservable();
        }

        protected IObservable<T> Observe(ChildQuery childQuery)
        {
            var realtimeDb = childQuery
                .AsRealtimeDatabase<T>(string.Empty, string.Empty, StreamingOptions.LatestOnly, InitialPullStrategy.Everything, true);

            realtimeDb.SyncExceptionThrown += (s, ex) => Console.WriteLine(ex.Exception);

            return realtimeDb
                .AsObservable()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        protected IObservable<U> ReadAllBasicType<U>(ChildQuery childQuery)
        {
            return childQuery
                .OnceAsync<U>()
                .ToObservable()
                .SelectMany(x => x)
                .Select(x => x.Object);
        }

        protected IObservable<U> ReadBasicType<U>(ChildQuery childQuery)
        {
            return childQuery
                .OnceSingleAsync<U>()
                .ToObservable();
        }

        private IObservable<T> ReadAll(RealtimeDatabase<T> realtimeDb)
        {
            return realtimeDb
                .Once()
                .ToObservable()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            MapKeyToId(firebaseObj.Object, firebaseObj.Key);
        }

        private void MapKeyToId(T obj, string key)
        {
            if(obj is BaseEntity model)
            {
                model.Id = key;
            }
        }
    }
}