using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Firebase.Analytics;
using TTKoreanSchool.Services;

namespace TTKoreanSchool.Android.Services
{
    public class AnalyticsService : BaseAnalyticsService
    {
        private readonly FirebaseAnalytics _firebaseAnalytics;

        public AnalyticsService(Context context)
        {
            _firebaseAnalytics = FirebaseAnalytics.GetInstance(context);
        }

        public override void SetUserId(string id)
        {
            _firebaseAnalytics.SetUserId(id);
        }

        public override void SetUserProperty(string name, string value)
        {
            _firebaseAnalytics.SetUserProperty(name, value);
        }

        protected override void LogEvent(string name, IDictionary<string, object> parameters)
        {
            Bundle bundle = new Bundle();
            foreach(var item in parameters)
            {
                if(item.Value.GetType() == typeof(string))
                {
                    bundle.PutString(item.Key, item.Value.ToString());
                }
                else if(item.Value.GetType() == typeof(bool))
                {
                    bundle.PutBoolean(item.Key, (bool)item.Value);
                }
            }

            _firebaseAnalytics.LogEvent(name, bundle);
        }
    }
}