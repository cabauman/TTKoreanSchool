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
using ReactiveUI;
using Splat;
using TTKoreanSchool.Android.Adapters;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "TTKoreanSchool.Android", Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity<IHomeViewModel>
    {
        private ProgressBar _progressBar;
        private GridView _gridView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_main);

            // PlayProgressAnimation();

            // var navService = Locator.Current.GetService<INavigationService>();
            // var llVocabSection = FindViewById<LinearLayout>(Resource.Id.ll_vocab_section);
            // llVocabSection.Click += (s, e) => navService.PushPage(new VocabScreenViewModel());

            _gridView = FindViewById<GridView>(Resource.Id.gridView1);
            _gridView.Adapter = new AppSectionAdapter(this, ViewModel.AppSections);
            _gridView.ItemClick += GridView_ItemClick;
        }

        private void GridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ViewModel.AppSections[e.Position].GoToSection.Execute().Subscribe();
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