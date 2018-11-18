using System.Reactive.Concurrency;

namespace TTKSCore
{
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        public IScheduler CurrentThread => Scheduler.CurrentThread;

        public IScheduler Dispatcher => DefaultScheduler.Instance; //DispatcherScheduler.Current;

        public IScheduler Immediate => Scheduler.Immediate;

        public IScheduler NewThread => NewThreadScheduler.Default;

        public IScheduler ThreadPool => Scheduler.Default; // Used to be Scheduler.ThreadPool

        public IScheduler TaskPool => TaskPoolScheduler.Default; // Used to be Scheduler.TaskPool
    }
}
