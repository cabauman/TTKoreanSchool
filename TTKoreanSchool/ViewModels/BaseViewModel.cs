using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using ReactiveUI;
using Splat;

namespace TTKoreanSchool.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        protected CompositeDisposable SubscriptionDisposables { get; } = new CompositeDisposable();
    }
}
