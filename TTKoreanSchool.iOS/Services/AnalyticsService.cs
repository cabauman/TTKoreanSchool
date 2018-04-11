using System.Collections.Generic;
using Firebase.Analytics;
using Foundation;
using TTKoreanSchool.Services;

namespace TTKoreanSchool.iOS.Services
{
    public class AnalyticsService : BaseAnalyticsService
    {
        public override void SetUserId(string id)
        {
            Analytics.SetUserID(id);
        }

        public override void SetUserProperty(string name, string value)
        {
            Analytics.SetUserProperty(name, value);
        }

        protected override void LogEvent(string name, IDictionary<string, object> parameters)
        {
            var dict = new NSDictionary<NSString, NSObject>();
            foreach(var item in parameters)
            {
                if(item.Value.GetType() == typeof(string))
                {
                    dict[item.Key] = new NSString(item.Value.ToString());
                }
                else if(item.Value.GetType() == typeof(bool))
                {
                    dict[item.Key] = new NSNumber((bool)item.Value);
                }
            }

            Analytics.LogEvent(name, dict);
        }
    }
}