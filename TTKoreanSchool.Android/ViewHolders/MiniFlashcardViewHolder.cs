using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace TTKoreanSchool.Android.ViewHolders
{
    public class MiniFlashcardViewHolder : RecyclerView.ViewHolder
    {
        public MiniFlashcardViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            Ko = itemView.FindViewById<TextView>(Resource.Id.tv_term_ko);
            Romanization = itemView.FindViewById<TextView>(Resource.Id.tv_term_romanization);

            itemView.Click += (sender, e) => listener(LayoutPosition);
        }

        public TextView Ko { get; private set; }

        public TextView Romanization { get; private set; }
    }
}