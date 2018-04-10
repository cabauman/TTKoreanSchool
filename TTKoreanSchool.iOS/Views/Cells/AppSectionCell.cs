using System;
using Foundation;
using ReactiveUI;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Views.Cells
{
    public partial class AppSectionCell : ReactiveCollectionViewCell<IButtonViewModel>
    {
        public static readonly NSString ReuseId = new NSString("AppSectionCell");
        public static readonly UINib Nib;

        private UIImageView _imageView;

        static AppSectionCell()
        {
            Nib = UINib.FromName(ReuseId, NSBundle.MainBundle);
        }

        protected AppSectionCell(IntPtr handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public UIImage Image
        {
            get { return _imageView.Image; }
            set { _imageView.Image = value; }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _imageView = new UIImageView(UIImage.FromFile("monkey.png"));
            _imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            _imageView.Center = ContentView.Center;
            AddSubview(_imageView);

            _imageView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraints = new[]
            {
                _imageView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -10f),
                _imageView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 10f),
                _imageView.TopAnchor.ConstraintEqualTo(TopAnchor, 10f),
                _imageView.BottomAnchor.ConstraintEqualTo(lblTitle.TopAnchor, -10f)
            };

            NSLayoutConstraint.ActivateConstraints(constraints);
        }
    }
}