using System.Reactive.Concurrency;

namespace TTKS.Core
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
