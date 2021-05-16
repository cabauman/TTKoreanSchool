using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using TTKS.Core.Models;
using TTKS.Core.Repositories;

namespace TTKS.Domain.Contracts.Repositories
{
    public interface ISentenceRepository
    {
        IObservable<Unit> Upsert(ExampleSentence item);

        IObservable<Unit> Upsert(IEnumerable<ExampleSentence> items);

        IObservable<Unit> Delete(string id);

        IObservable<Unit> Delete(IEnumerable<ExampleSentence> items);

        IObservable<ExampleSentence> Get(string id);

        IObservable<ExampleSentence> GetAll(bool forceRefresh = false);

        IObservable<ChangeEvent<ExampleSentence>> Bind<TResult>(
            out ReadOnlyObservableCollection<TResult> list,
            Func<ExampleSentence, TResult> selector);
    }
}
