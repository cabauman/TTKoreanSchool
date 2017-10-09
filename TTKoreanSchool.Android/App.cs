using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Android.Activities;
using TTKoreanSchool.Android.Services;
using TTKoreanSchool.Services;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;
using static Android.App.Application;

namespace TTKoreanSchool.Android
{
    [Application(Theme = "@android:style/Theme.Material.Light")]
    public class App : Application, IActivityLifecycleCallbacks, IEnableLogger
    {
        private readonly Subject<Activity> _activityCreatedSubject;
        private readonly Subject<Activity> _currentActivitySubject;

        public App(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            _activityCreatedSubject = new Subject<Activity>();
            _currentActivitySubject = new Subject<Activity>();

            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();
            RegisterPages();
            RegisterServices();
        }

        public Activity CurrentActivity { get; private set; }

        public IObservable<Activity> ActivityCreatedObservable
        {
            get { return _activityCreatedSubject as IObservable<Activity>; }
        }

        public IObservable<Activity> CurrentActivityObservable
        {
            get { return _currentActivitySubject as IObservable<Activity>; }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CurrentActivity = activity;
            _currentActivitySubject.OnNext(activity);
            _activityCreatedSubject.OnNext(activity);
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
            _currentActivitySubject.OnNext(activity);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        private void RegisterPages()
        {
            Locator.CurrentMutable.Register(() => new MainActivity(), typeof(IViewFor<IHomeViewModel>));
            Locator.CurrentMutable.Register(() => new HangulSectionActivity(), typeof(IViewFor<IHangulZoneViewModel>));
            Locator.CurrentMutable.Register(() => new VocabSectionActivity(), typeof(IViewFor<IHangulZoneViewModel>));
            Locator.CurrentMutable.Register(() => new GrammarSectionActivity(), typeof(IViewFor<IGrammarZoneViewModel>));
            Locator.CurrentMutable.Register(() => new ConjugatorActivity(), typeof(IViewFor<IConjugatorViewModel>));
            Locator.CurrentMutable.Register(() => new StudentPortalActivity(), typeof(IViewFor<IStudentPortalViewModel>));
            Locator.CurrentMutable.Register(() => new VideoFeedActivity(), typeof(IViewFor<IVideoFeedViewModel>));
        }

        private void RegisterServices()
        {
            var navService = new NavigationService(ActivityCreatedObservable, CurrentActivityObservable);
            Locator.CurrentMutable.RegisterConstant(navService, typeof(INavigationService));
            Locator.CurrentMutable.RegisterConstant(new AndroidLoggingService(), typeof(ILogger));
        }
    }
}