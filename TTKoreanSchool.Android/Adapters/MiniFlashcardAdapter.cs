//using System;
//using System.Collections.Generic;
//using Android.Support.V7.Widget;
//using Android.Views;
//using TTKoreanSchool.Android.ViewHolders;
//using TTKoreanSchool.ViewModels;

//namespace TTKoreanSchool.Android.Adapters
//{
//    public class MiniFlashcardAdapter : RecyclerView.Adapter
//    {
//        // Underlying data set (a photo album):
//        private IList<MiniFlashcardViewModel> _miniFlashcardVms;

//        // Load the adapter with the data set (photo album) at construction time:
//        public MiniFlashcardAdapter(IList<MiniFlashcardViewModel> miniFlashcardVms)
//        {
//            _miniFlashcardVms = miniFlashcardVms;
//        }

//        // Event handler for item clicks:
//        public event EventHandler<int> ItemClick;

//        // Return the number of photos available in the photo album:
//        public override int ItemCount
//        {
//            get { return _miniFlashcardVms.Count; }
//        }

//        // Create a new photo CardView (invoked by the layout manager):
//        public override RecyclerView.ViewHolder
//            OnCreateViewHolder(ViewGroup parent, int viewType)
//        {
//            // Inflate the CardView for the photo:
//            View itemView = LayoutInflater.From(parent.Context).
//                        Inflate(Resource.Layout.PhotoCardView, parent, false);

//            // Create a ViewHolder to find and hold these view references, and
//            // register OnClick with the view holder:
//            MiniFlashcardViewHolder vh = new PhotoViewHolder(itemView, OnClick);
//            return vh;
//        }

//        // Fill in the contents of the photo card (invoked by the layout manager):
//        public override void
//            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
//        {
//            MiniFlashcardViewHolder vh = holder as MiniFlashcardViewHolder;

//            // Set the ImageView and TextView in this ViewHolder's CardView
//            // from this position in the photo album:
//            vh.Image.SetImageResource(_miniFlashcardVms[position].PhotoID);
//            vh.Caption.Text = _miniFlashcardVms[position].Caption;
//        }

//        // Raise an event when the item-click takes place:
//        private void OnClick(int position)
//        {
//            ItemClick?.Invoke(this, position);
//        }
//    }
//}