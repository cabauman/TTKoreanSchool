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
using Java.Lang;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Android.Adapters;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "VocabZoneActivity")]
    public class VocabZoneActivity : BaseActivity<IVocabZoneLandingPageViewModel>
    {
        private static SectionedRecyclerViewAdapter _sectionAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_vocab_zone);

            ViewModel.WhenAnyValue(vm => vm.LoadSections)
                .SelectMany(x => x.Execute())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    vocabSections =>
                    {
                        InitSections(vocabSections);
                    },
                    ex =>
                    {
                        Console.WriteLine(ex);
                    })
                .DisposeWith(SubscriptionDisposables);
        }

        private void InitSections(IList<IVocabSectionViewModel> sections)
        {
            _sectionAdapter = new SectionedRecyclerViewAdapter();

            for(int i = 0; i < sections.Count; ++i)
            {
                int sectionPos = i;
                var sectionView = new VocabSectionView(sections[i].Title, sections[i].Children, this);
                sectionView.ItemClick += (sender, position) => ItemClickHandler(sectionPos, position);

                _sectionAdapter.AddSection(sectionView);
            }

            var rv = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            GridLayoutManager layoutMgr = new GridLayoutManager(this, 2);
            layoutMgr.SetSpanSizeLookup(new SpanSizeLookup(_sectionAdapter));
            rv.SetLayoutManager(layoutMgr);
            rv.SetAdapter(_sectionAdapter);
        }

        private void ItemClickHandler(int sectionPos, int position)
        {
            int posInSection = _sectionAdapter.GetPositionInSection(position);
            //Toast.MakeText(
            //    Application.Context,
            //    string.Format("Clicked on position #{0} of Section {1}", posInSection, sectionTitle),
            //    ToastLength.Short)
            //        .Show();



            ViewModel.Sections[sectionPos].Children[posInSection].Selected();
        }



        public int GetSectionPosition(Section section)
        {
            int currentPos = 0;

            foreach(var entry in _sectionAdapter.CopyOfSectionsMap.Values)
            {
                if(!entry.Visible)
                {
                    continue;
                }

                if(entry == section)
                {
                    return currentPos;
                }

                currentPos += 1;
            }

            throw new IllegalArgumentException("Invalid section");
        }



        public class SpanSizeLookup : GridLayoutManager.SpanSizeLookup
        {
            private readonly SectionedRecyclerViewAdapter _adapter;

            public SpanSizeLookup(SectionedRecyclerViewAdapter adapter)
            {
                _adapter = adapter;
            }

            public override int GetSpanSize(int position)
            {
                var itemType = _adapter.GetItemViewType(position);
                switch(_adapter.GetSectionItemViewType(position))
                {
                    case SectionedRecyclerViewAdapter.ViewTypeHeader:
                        return 2;
                    default:
                        return 1;
                }
            }
        }
    }
}