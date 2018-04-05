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
    public class VocabSectionHeaderViewHolder : RecyclerView.ViewHolder
    {
        public VocabSectionHeaderViewHolder(View view)
            : base(view)
        {
            Title = view.FindViewById<TextView>(Resource.Id.tv_title);
        }

        public TextView Title { get; }
    }
}