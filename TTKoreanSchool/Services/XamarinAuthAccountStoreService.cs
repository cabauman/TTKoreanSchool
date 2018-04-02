using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using Xamarin.Auth;

namespace TTKoreanSchool.Services
{
    public class XamarinAuthAccountStoreService : IAccountStoreService
    {
        public TongTongAccount CurrentAccount { get; set; }

        public IObservable<Unit> Delete(TongTongAccount account, string serviceId)
        {
            return AccountStore
                .Create()
                .DeleteAsync(account.XamarinAuthAccount, serviceId)
                .ToObservable();
        }

        public IObservable<IList<TongTongAccount>> FindAccountsForService(string serviceId)
        {
            return AccountStore
                .Create()
                .FindAccountsForServiceAsync(serviceId)
                .ToObservable()
                .SelectMany(account => account)
                .Select(account => new TongTongAccount(account))
                .ToList();
        }

        public IObservable<Unit> Save(TongTongAccount account, string serviceId)
        {
            return AccountStore
                .Create()
                .SaveAsync(account.XamarinAuthAccount, serviceId)
                .ToObservable();
        }

        public IObservable<Unit> SaveFirebaseAuthJson(string json)
        {
            CurrentAccount.FirebaseAuthJson = json;
            return Save(CurrentAccount, "Firebase");
        }
    }
}