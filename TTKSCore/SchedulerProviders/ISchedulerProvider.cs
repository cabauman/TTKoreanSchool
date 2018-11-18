using System.Reactive.Concurrency;

namespace TTKSCore
{
    public interface ISchedulerProvider
    {
        IScheduler CurrentThread { get; }

        IScheduler Dispatcher { get; }

        IScheduler Immediate { get; }

        IScheduler NewThread { get; }

        IScheduler ThreadPool { get; }
    }
}
