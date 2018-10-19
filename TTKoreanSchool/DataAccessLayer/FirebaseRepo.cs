﻿extern alias SplatAlias;

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseRepo<T> : IEnableLogger
        where T : class
    {
        protected RealtimeDatabase<T> RealtimeDb { get; set; }

        protected IObservable<T> ReadAll(ChildQuery childQuery, string filenameModifier = "")
        {
            RealtimeDb = childQuery
                .AsRealtimeDatabase<T>(filenameModifier, string.Empty, StreamingOptions.LatestOnly, InitialPullStrategy.Everything, true);

            RealtimeDb.SyncExceptionThrown +=
                (s, ex) =>
                {
                    this.Log().Error(ex.Exception);
                };

            return RealtimeDb
                .PullAsync()
                .ToObservable()
                .SelectMany(_ => ReadAll(RealtimeDb));
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

        protected IObservable<U> ReadAllBasicType<U>(ChildQuery childQuery, bool useCache = true)
            where U : class
        {
            var realtimeDb = childQuery
                .AsRealtimeDatabase<U>(string.Empty, string.Empty, StreamingOptions.LatestOnly, InitialPullStrategy.Everything, true);

            realtimeDb.SyncExceptionThrown +=
                (s, ex) =>
                {
                    Console.WriteLine(ex.Exception);
                };

            if(!useCache || realtimeDb.Database?.Count == 0)
            {
                return realtimeDb
                    .PullAsync()
                    .ToObservable()
                    .SelectMany(_ => ReadAllBasicType(realtimeDb));
            }

            return ReadAllBasicType(realtimeDb);
        }

        protected IObservable<U> ReadBasicType<U>(ChildQuery childQuery)
        {
            return childQuery
                .OnceSingleAsync<U>()
                .ToObservable();
        }

        protected IObservable<T> ReadAll(RealtimeDatabase<T> realtimeDb)
        {
            return realtimeDb.Database
                .Once()
                .ToObservable()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        private IObservable<U> ReadAllBasicType<U>(RealtimeDatabase<U> realtimeDb)
            where U : class
        {
            return realtimeDb
                .Once()
                .ToObservable()
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