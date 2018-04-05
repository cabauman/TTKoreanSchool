using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace TTKoreanSchool.Android.ViewHolders
{
    public class VocabSectionItemViewHolder : RecyclerView.ViewHolder
    {
        public VocabSectionItemViewHolder(View view, Action<int> listener)
            : base(view)
        {
            RootView = view;
            Icon = view.FindViewById<ImageView>(Resource.Id.img_icon);
            Title = view.FindViewById<TextView>(Resource.Id.tv_title);

            view.Click += (sender, e) => listener(AdapterPosition);
        }

        public View RootView { get; }

        public ImageView Icon { get; }

        public TextView Title { get; }
    }
}