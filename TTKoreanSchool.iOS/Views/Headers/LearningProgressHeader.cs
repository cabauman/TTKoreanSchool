using System;
using System.Drawing;
using CoreGraphics;
using DACircularProgress;
using Foundation;
using Splat;
using TTKoreanSchool.iOS.Utils;
using TTKoreanSchool.Utils;
using UIKit;

namespace TTKoreanSchool.iOS.Views.Headers
{
    public class LearningProgressHeader : UICollectionReusableView
    {
        public static readonly NSString Key = new NSString("LearningProgressHeader");

        private UILabel _label;

        [Export("initWithFrame:")]
        public LearningProgressHeader(CGRect frame)
            : base(frame)
        {
            BackgroundColor = UIColor.White;

            // label = new UILabel() { Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width, 200), BackgroundColor = UIColor.Yellow };
            // AddSubview(label);
            var length = 150f;
            var x = (Bounds.Width / 2f) - (length / 2);
            var y = (Bounds.Height / 2f) - (length / 2);
            var rect = new CGRect(x, y, length, length);

            var progressView = new LabeledCircularProgressView(rect)
            {
                RoundedCorners = true,
                ThicknessRatio = 0.1f,
                TrackTintColor = UIColor.Black,
                ProgressTintColor = ColorPalette.Amber.ToNative()
            };

            progressView.ProgressLabel.Center = new CGPoint(progressView.ProgressLabel.Center.X, progressView.ProgressLabel.Center.Y + 30f);
            progressView.ProgressLabel.Font = progressView.ProgressLabel.Font.WithSize(32f);
            progressView.ProgressLabel.Text = "10%";
            progressView.SetProgress(0.5f, true, 1d, 2d);

            AddSubview(progressView);
        }

        public string Text
        {
            get
            {
                return _label.Text;
            }

            set
            {
                _label.Text = value;
                SetNeedsDisplay();
            }
        }
    }
}