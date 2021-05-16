using Firebase.Database;
using Firebase.Database.Offline;
using GameCtor.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TTKS.Core.Models;

namespace TTKS.Core.Repositories
{
    public interface IMapper<in T1, out T2> { T2 Map(T1 input); };


    public interface IReadOnlyRepository<T>
    {
        IObservable<T> Get(string id);

        IObservable<ChangeEvent<T>> ChangeStream { get; }

        IObservable<Unit> RefreshIfExpired();

        IObservable<Unit> ForceRefresh();
    }

    public class DummyRepo<TDto, TModel>
    {
        private IMapper<TDto, TModel> mapper;
        private IEnumerable<TModel> cache;

        public DummyRepo(IMapper<TDto, TModel> mapper)
        {
            this.AsObservable<TModel>().Bind(out var list, x => "").Subscribe();
            this.AsObservable()
                .Transform(x => 1)
                .Bind(out var collection)
                .Subscribe();
        }

        public TModel Get(string id)
        {
            TDto dto = default;
            return mapper.Map(dto);
        }

        private Subject<ChangeEvent<TModel>> subject;
        public IObservable<ChangeEvent<TModel>> AsObservable<T>()
        {
            return subject
                .StartWith(
                    cache
                        .Select(
                            x => new ChangeEvent<TModel>(x, ChangeEventType.Add)));
        }

        public IObservable<ChangeEvent<TModel>> AsObservable()
        {
            return subject.StartWith(cache.Select(x => new ChangeEvent<TModel>(x, ChangeEventType.Add)));
        }
    }

    public enum ChangeEventType
    {
        Add,
        Remove,
        Update,
    }

    public class ChangeEvent<T>
    {
        public ChangeEvent(T item, ChangeEventType changeEventType)
        {
            Item = item;
            ChangeEventType = changeEventType;
        }

        public T Item { get; }

        public ChangeEventType ChangeEventType { get; }
    }

    public static class RepoExtensions
    {
        public static IObservable<ChangeEvent<TIn>> Bind<TIn, TOut>(
            this IObservable<ChangeEvent<TIn>> @this,
            out ReadOnlyObservableCollection<TOut> collection,
            Func<TIn, TOut> transformer)
            //where TIn : IModel
        {
            var inner = new ObservableCollection<TOut>();
            collection = new ReadOnlyObservableCollection<TOut>(inner);
            return Observable.Create<ChangeEvent<TIn>>(observer =>
            {
                return @this
                    .Select(
                        x =>
                        {
                            var transformedItem = transformer.Invoke(x.Item);

                            switch (x.ChangeEventType)
                            {
                                case ChangeEventType.Add:
                                    inner.Add(transformedItem);
                                    break;
                                case ChangeEventType.Remove:
                                    inner.Remove(transformedItem);
                                    break;
                                default:
                                    throw new Exception();
                            }

                            return x;
                        })
                    .Subscribe(observer);
            });
        }

        public static IObservable<ChangeEvent<TOut>> Transform<TIn, TOut>(this IObservable<ChangeEvent<TIn>> @this, Func<TIn, TOut> transformer)
        {
            if (transformer == null)
                throw new ArgumentNullException(nameof(transformer));

            return @this
                .Select(x => new ChangeEvent<TOut>(transformer.Invoke(x.Item), x.ChangeEventType));
        }

        public static IObservable<ChangeEvent<T>> Bind<T>(this IObservable<ChangeEvent<T>> @this, out ReadOnlyObservableCollection<T> collection)
        {
            var inner = new ObservableCollection<T>();
            collection = new ReadOnlyObservableCollection<T>(inner);
            return Observable.Create<ChangeEvent<T>>(observer =>
            {
                return @this
                    .Select(
                        x =>
                        {
                            switch (x.ChangeEventType)
                            {
                                case ChangeEventType.Add:
                                    inner.Add(x.Item);
                                    break;
                                case ChangeEventType.Remove:
                                    inner.Remove(x.Item);
                                    break;
                                default:
                                    throw new Exception();
                            }

                            return x;
                        })
                    .Subscribe(observer);
            });
        }
    }
}
