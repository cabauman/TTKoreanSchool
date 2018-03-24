using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace TTKoreanSchool.Android.ViewHolders
{
    public class MiniFlashcardViewHolder : RecyclerView.ViewHolder
    {
        // Get references to the views defined in the CardView layout.
        public MiniFlashcardViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            // Locate and cache view references:
            //Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            //Caption = itemView.FindViewById<TextView>(Resource.Id.textView);

            // Detect user clicks on the item view and report which item
            // was clicked (by layout position) to the listener:
            itemView.Click += (sender, e) => listener(LayoutPosition);
        }


        public ImageView Image { get; private set; }

        public TextView Caption { get; private set; }
    }
}