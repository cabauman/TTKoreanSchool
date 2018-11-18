using System;

namespace TTKoreanSchool.Tests
{
    public class LeakMonitor<T>
        where T : class
    {
        private readonly WeakReference _reference;

        public LeakMonitor(T itemToWatch)
        {
            _reference = new WeakReference(itemToWatch);
        }

        public T Item => _reference.Target as T;

        public bool IsItemAlive()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return _reference.IsAlive;
        }
    }
}
