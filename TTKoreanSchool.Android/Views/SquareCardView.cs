using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TTKoreanSchool.Android.Views
{
    [Register("ttkoreanschool/android/views/SquareCardView")]
    public class SquareCardView : CardView
    {
        public SquareCardView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public SquareCardView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, widthMeasureSpec);
        }
    }
}