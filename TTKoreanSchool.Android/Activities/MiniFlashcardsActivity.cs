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
using IO.Github.Luizgrp.Sectionedrecyclerviewadapter;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Android.Adapters;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "MiniFlashcardsActivity")]
    public class MiniFlashcardsActivity : BaseActivity<IMiniFlashcardsPageViewModel>
    {
        private static MiniFlashcardAdapter _adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_vocab_zone);

            ViewModel.WhenAnyValue(vm => vm.LoadVocabTerms)
                .SelectMany(x => x.Execute())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    flashcards =>
                    {
                        InitFlashcards(flashcards);
                    },
                    ex =>
                    {
                        Console.WriteLine(ex);
                    })
                .DisposeWith(SubscriptionDisposables);
        }

        private void InitFlashcards(IList<IMiniFlashcardViewModel> flashcards)
        {
            _adapter = new MiniFlashcardAdapter(flashcards);
            _adapter.ItemClick += ItemClickHandler;
            var rv = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            rv.SetLayoutManager(new LinearLayoutManager(this));
            rv.SetAdapter(_adapter);
        }

        private void ItemClickHandler(object sender, int position)
        {
            Toast.MakeText(
                Application.Context,
                string.Format("Clicked on position #{0}", position),
                ToastLength.Short)
                    .Show();
        }
    }
}