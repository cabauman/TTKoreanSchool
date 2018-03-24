using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "VocabZoneActivity")]
    public class VocabZoneActivity : BaseActivity<IVocabZoneLandingPageViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_vocab_zone);

            var rv = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutMgr = new LinearLayoutManager(this);
            rv.SetLayoutManager(layoutMgr);

            //var adapter = new MiniFlashcard(mPhotoAlbum);

            //this.WhenAnyValue(x => x.ViewModel.LoadSections)
            //    .SelectMany(x => x.Execute())
            //    .Subscribe(x =>
            //    {
            //        Console.WriteLine("It works!");
            //    });

            //ViewModel.WhenAnyValue(vm => vm.Sections)
            //    .Subscribe(
            //        sections =>
            //        {
            //            Console.WriteLine("It works!");
            //        },
            //        error =>
            //        {
            //            this.Log().Error(error.Message);
            //        },
            //        () =>
            //        {
            //            this.Log().Debug("Complete!");
            //        })
            //    .DisposeWith(SubscriptionDisposables);
        }
    }
}