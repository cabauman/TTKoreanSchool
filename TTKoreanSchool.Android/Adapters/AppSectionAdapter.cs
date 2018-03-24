using System;
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

namespace TTKoreanSchool.Android.Adapters
{
    public class AppSectionAdapter : BaseAdapter
    {
        private Context _context;
        private ButtonViewModel[] _appSections;

        public AppSectionAdapter(Context context, ButtonViewModel[] appSections)
        {
            _context = context;
            _appSections = appSections;
        }

        public override int Count
        {
            get { return _appSections.Length; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            AppSectionAdapterViewHolder holder = null;

            if(view != null)
            {
                holder = view.Tag as AppSectionAdapterViewHolder;
            }

            if(holder == null)
            {
                holder = new AppSectionAdapterViewHolder();
                var inflater = _context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.item_app_section, parent, false);
                holder.Title = view.FindViewById<TextView>(Resource.Id.tv_app_section_title);
                holder.Image = view.FindViewById<ImageView>(Resource.Id.img_app_section_image);
                view.Tag = holder;
            }

            var data = _appSections[position];
            holder.Title.Text = data.Title;

            // int imgResourceId = _context.Resources.GetIdentifier(data.ImageName.ToLower(), "drawable", _context.PackageName);
            // Reflection version
            int imgResourceId = (int)typeof(Resource.Drawable).GetField(data.ImageName).GetValue(null);
            holder.Image.SetImageResource(imgResourceId);

            return view;
        }
    }

    public class AppSectionAdapterViewHolder : Java.Lang.Object
    {
        public TextView Title { get; set; }

        public ImageView Image { get; set; }
    }
}