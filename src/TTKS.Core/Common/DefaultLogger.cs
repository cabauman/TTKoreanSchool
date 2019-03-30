using System;
using System.ComponentModel;
using System.Diagnostics;
using Splat;

namespace TTKS.Core.Common
{
    public class DefaultLogger : ILogger
    {
        public void Write(string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)Level) return;
            Debug.WriteLine(message);
        }

        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            Write(message, logLevel);
        }

        public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public LogLevel Level { get; set; }
    }
}
