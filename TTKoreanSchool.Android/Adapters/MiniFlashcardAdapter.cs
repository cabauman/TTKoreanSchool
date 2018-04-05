using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using TTKoreanSchool.Android.ViewHolders;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Adapters
{
    public class MiniFlashcardAdapter : RecyclerView.Adapter
    {
        private IList<IMiniFlashcardViewModel> _miniFlashcardVms;

        public MiniFlashcardAdapter(IList<IMiniFlashcardViewModel> miniFlashcardVms)
        {
            _miniFlashcardVms = miniFlashcardVms;
        }

        public event EventHandler<int> ItemClick;

        public override int ItemCount
        {
            get { return _miniFlashcardVms?.Count ?? 0; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.item_mini_flashcard, parent, false);

            MiniFlashcardViewHolder vh = new MiniFlashcardViewHolder(itemView, OnClick);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MiniFlashcardViewHolder vh = holder as MiniFlashcardViewHolder;

            vh.Ko.Text = _miniFlashcardVms[position].Ko;
            vh.Romanization.Text = _miniFlashcardVms[position].Romanization;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}