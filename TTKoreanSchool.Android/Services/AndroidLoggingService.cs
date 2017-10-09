using System.Globalization;
using Splat;
using TTKoreanSchool.Services;

namespace TTKoreanSchool.Android.Services
{
    public class AndroidLoggingService : LoggingService
    {
        protected override void Output(string message, LogLevel logLevel)
        {
            global::Android.Util.Log.WriteLine(ToLogPriority(logLevel), string.Empty, message.ToString(CultureInfo.InvariantCulture));
        }

        private static global::Android.Util.LogPriority ToLogPriority(LogLevel level)
        {
            switch(level)
            {
                case LogLevel.Debug:
                    return global::Android.Util.LogPriority.Debug;
                case LogLevel.Info:
                    return global::Android.Util.LogPriority.Info;
                case LogLevel.Warn:
                    return global::Android.Util.LogPriority.Warn;
                case LogLevel.Error:
                    return global::Android.Util.LogPriority.Error;
                default:
                    return global::Android.Util.LogPriority.Verbose;
            }
        }
    }
}