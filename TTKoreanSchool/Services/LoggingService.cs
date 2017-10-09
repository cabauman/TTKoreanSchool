using System.Diagnostics;
using Splat;

namespace TTKoreanSchool.Services
{
    public class LoggingService : ILogger
    {
        public LogLevel Level { get; set; }

        public void Write(string message, LogLevel logLevel)
        {
            if((int)logLevel < (int)Level)
            {
                return;
            }

            switch(logLevel)
            {
                case LogLevel.Warn:
                case LogLevel.Error:
                case LogLevel.Fatal:
                case LogLevel.Debug:
                case LogLevel.Info:
                default:
                    Output(message, logLevel);
                    break;
            }
        }

        protected virtual void Output(string message, LogLevel logLevel)
        {
            Debug.WriteLine(message);
        }
    }
}
