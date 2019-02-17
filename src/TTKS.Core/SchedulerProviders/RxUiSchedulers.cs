using System.Reactive.Concurrency;
using ReactiveUI;

namespace TTKS.Core
{
    public sealed class RxUiSchedulers : ISchedulerProvider
    {
        public IScheduler CurrentThread => Scheduler.CurrentThread;

        public IScheduler Dispatcher => RxApp.MainThreadScheduler;

        public IScheduler Immediate => Scheduler.Immediate;

        public IScheduler NewThread => NewThreadScheduler.Default;

        public IScheduler ThreadPool => Scheduler.Default;

        public IScheduler TaskPool => RxApp.TaskpoolScheduler;
    }
}
