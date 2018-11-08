using System.Diagnostics;
using Splat;

namespace TTKoreanSchool.Common
{
    public class DefaultLogger : ILogger
    {
        public void Write(string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level) return;
            Debug.WriteLine(message);
        }

        public LogLevel Level { get; set; }
    }
}
