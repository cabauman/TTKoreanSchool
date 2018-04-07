using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Gms.Common.Images;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IO.Github.Luizgrp.Sectionedrecyclerviewadapter;
using TTKoreanSchool.Android.ViewHolders;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android
{
    public class VocabSectionView : StatelessSection
    {
        private readonly ImageManager _imageManager;

        private string _title;
        private IReadOnlyList<IVocabSectionChildViewModel> _children;

        public VocabSectionView(string title, IReadOnlyList<IVocabSectionChildViewModel> children, Context context)
            : base(SectionParameters.InvokeBuilder()
                .ItemResourceId(Resource.Layout.Item_VocabSection)
                .HeaderResourceId(Resource.Layout.Header_VocabSection)
                .Build())
        {
            _title = title;
            _children = children;
            _imageManager = ImageManager.Create(context);
        }

        public event EventHandler<int> ItemClick;

        public override int ContentItemsTotal => _children?.Count ?? 0;

        public override RecyclerView.ViewHolder GetItemViewHolder(View view)
        {
            return new VocabSectionItemViewHolder(view, OnClick);
        }

        public override void OnBindItemViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var itemHolder = holder as VocabSectionItemViewHolder;

            var section = _children[position];

            itemHolder.Title.Text = section.Title;
            //itemHolder.Icon.SetImageResource(Resource.Drawable.Icon_StudentPortal);
        }

        public override RecyclerView.ViewHolder GetHeaderViewHolder(View view)
        {
            return new VocabSectionHeaderViewHolder(view);
        }

        public override void OnBindHeaderViewHolder(RecyclerView.ViewHolder holder)
        {
            VocabSectionHeaderViewHolder headerHolder = (VocabSectionHeaderViewHolder)holder;

            headerHolder.Title.Text = _title;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}