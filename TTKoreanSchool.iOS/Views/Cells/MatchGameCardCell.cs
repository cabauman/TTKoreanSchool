using System;
using System.Drawing;
using System.Reactive.Linq;
using CoreGraphics;
using Foundation;
using ReactiveUI;
using Splat;
using TTKoreanSchool.iOS.Extensions;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Views.Cells
{
    public class MatchGameCardCell : ReactiveCollectionViewCell<IMatchGameCardViewModel>
    {
        public static readonly NSString ReuseId = new NSString("MatchGameCardCell");

        private const float LABEL_PADDING = 5f;

        private readonly UILabel _textLabel;

        [Export("initWithFrame:")]
        public MatchGameCardCell(CGRect frame)
            : base(frame)
        {
            SetContentViewStyle();
            _textLabel = new UILabel();
            SetFontStyle(_textLabel);
            AddSubview(_textLabel);
        }

        public string Text
        {
            get
            {
                return _textLabel.Text;
            }

            set
            {
                _textLabel.Text = value;
                _textLabel.AdjustFontSizeToHeight();
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _textLabel.Frame = GetPaddedLabelFrame();
        }

        public void InitViewModel(IMatchGameCardViewModel viewModel)
        {
            ViewModel = viewModel;

            this.OneWayBind(ViewModel, vm => vm.Hidden, v => v.Hidden);
            this.OneWayBind(ViewModel, vm => vm.Color, v => v.ContentView.BackgroundColor, color => color.ToNative());
            this.OneWayBind(ViewModel, vm => vm.BorderColor, v => v.ContentView.Layer.BorderColor, color => color.ToNative().CGColor);
            this.OneWayBind(ViewModel, vm => vm.Text, v => v.Text);
        }

        private CGRect GetPaddedLabelFrame()
        {
            CGRect labelFrame = Bounds;
            labelFrame.Width -= LABEL_PADDING * 2;
            labelFrame.Height -= LABEL_PADDING * 2;
            labelFrame.X += LABEL_PADDING;
            labelFrame.Y += LABEL_PADDING;

            return labelFrame;
        }

        private void SetContentViewStyle()
        {
            ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
            ContentView.Layer.BorderWidth = 2.0f;
            ContentView.Layer.CornerRadius = 3;
        }

        private void SetFontStyle(UILabel label)
        {
            label.BackgroundColor = UIColor.Clear;
            label.TextAlignment = UITextAlignment.Center;
            label.LineBreakMode = UILineBreakMode.WordWrap;
            label.Lines = 0;
            label.Text = "안녕";
        }
    }
}