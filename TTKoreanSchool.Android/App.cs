using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Splat;
using static Android.App.Application;

namespace TTKoreanSchool.Android
{
    [Application(Theme = "@android:style/Theme.Material.Light")]
    public class App : Application, IActivityLifecycleCallbacks, IEnableLogger
    {
        public App(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            var bootstrapper = new AndroidBootstrapper();
            bootstrapper.Run();
        }

        public static Activity CurrentActivity { get; private set; }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CurrentActivity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CurrentActivity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CurrentActivity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}