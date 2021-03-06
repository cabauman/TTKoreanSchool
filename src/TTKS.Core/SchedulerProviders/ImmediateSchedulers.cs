﻿using System.Reactive.Concurrency;

namespace TTKS.Core
{
    public sealed class ImmediateSchedulers : ISchedulerProvider
    {
        public IScheduler CurrentThread => Scheduler.Immediate;

        public IScheduler Dispatcher => Scheduler.Immediate;

        public IScheduler Immediate => Scheduler.Immediate;

        public IScheduler NewThread => Scheduler.Immediate;

        public IScheduler ThreadPool => Scheduler.Immediate;
    }
}
