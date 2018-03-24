using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Firebase.Database.Offline;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Android.Adapters;
using TTKoreanSchool.DataAccessLayer;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;
using System.Reactive.Threading.Tasks;
using System.Collections.Generic;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "TTKoreanSchool.Android", Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity<IHomePageViewModel>
    {
        private ProgressBar _progressBar;
        private GridView _gridView;
        private IDisposable subscription;
        private IObservable<IList<Message>> observable;

        public class Student
        {
            public Message msg1 { get; set; }
            public Message msg2 { get; set; }
        }

        public class Message
        {
            public string message { get; set; }
            public string sender { get; set; }
            public string time { get; set; }
        }

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_home);

            //var options = new FirebaseOptions()
            //{
            //    OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
            //};

            //var client = new FirebaseClient("https://tt-korean-academy.firebaseio.com/", options);
            //var messagesDb = client.Child("messages/student1").AsRealtimeDatabase<Message>("", "", StreamingOptions.LatestOnly, InitialPullStrategy.Everything, true);

            //messagesDb.SyncExceptionThrown += (s, ex) => Console.WriteLine(ex.Exception);

            //var messages = messagesDb
            //    .Once();

            //Console.WriteLine(messages?.ToString());

            //var observable = messagesDb
            //    .AsObservable()
            //    .Where(m => m != null && m.Object != null)
            //    .Subscribe(m =>
            //    {
            //        Console.WriteLine(m.Object.message);
            //    });

            //observable = client
            //    .Child("messages/student1")
            //    .OnceAsync<Message>()
            //    .ToObservable()
            //    .SelectMany(x => x)
            //    .Select(x => x.Object)
            //    .ToList();
            //subscription = observable
            //    .Subscribe(m =>
            //    {
            //        Console.WriteLine(m);
            //    });

            //observable = client
            //    .Child("messages/student1")
            //    .AsObservable<Message>()
            //    .Where(m => m != null && m.Object != null)
            //    .Select(x => x.Object);
            //subscription = observable
            //    .Subscribe(m =>
            //    {
            //        Console.WriteLine(m);
            //    });

            //await messagesDb.PullAsync();
            //var messages = messagesDb.Once();
            //.Child("messages")
            //.OnceAsync<Student>();

            // PlayProgressAnimation();

            var navService = Locator.Current.GetService<INavigationService>();
            var llVocabSection = FindViewById<LinearLayout>(Resource.Id.ll_vocab_section);
            llVocabSection.Click += (s, e) => navService.PushPage(new VocabZoneLandingPageViewModel());
            //llVocabSection.Click += (s, e) =>
            //{
            //    Console.WriteLine(subscription.ToString());
            //};

            //_gridView = FindViewById<GridView>(Resource.Id.gridView1);
            //_gridView.Adapter = new AppSectionAdapter(this, ViewModel.AppSections);
            //_gridView.ItemClick += GridView_ItemClick;
        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ViewModel.AppSections[e.Position].Command.Execute().Subscribe();
        }

        private void PlayProgressAnimation()
        {
            _progressBar = this.FindViewById<ProgressBar>(Resource.Id.progressBar);
            ObjectAnimator animation = ObjectAnimator.OfInt(_progressBar, "secondaryProgress", 100, 400);
            animation.SetDuration(2000);
            animation.SetInterpolator(new DecelerateInterpolator());
            animation.Start();
        }
    }
}