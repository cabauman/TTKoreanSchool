﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Android.Activities
{
    [Activity(Label = "ConjugatorActivity")]
    public class ConjugatorActivity : BaseActivity<IConjugatorViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}