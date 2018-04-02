using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IAccountStoreService
    {
        TongTongAccount CurrentAccount { get; set; }

        IObservable<Unit> Delete(TongTongAccount account, string serviceId);

        IObservable<IList<TongTongAccount>> FindAccountsForService(string serviceId);

        IObservable<Unit> Save(TongTongAccount account, string serviceId);

        IObservable<Unit> SaveFirebaseAuthJson(string json);
    }
}