using CoreGraphics;
using Foundation;
using UIKit;

namespace TTKoreanSchool.iOS.Extensions
{
    public static class UILabelExtensions
    {
        public static void AdjustFontSizeToHeight(this UILabel label)
        {
            var words = label.Text.Split(' ');
            string longestWord = words[0];
            for(int i = 1; i < words.Length; ++i)
            {
                if(words[i].Length > longestWord.Length)
                {
                    longestWord = words[i];
                }
            }

            NSString nsString = new NSString(longestWord);
            const int minFontSize = 10;
            int size = 32;
            for(; size >= minFontSize; --size)
            {
                var font = label.Font.WithSize(size);

                UIStringAttributes attribs = new UIStringAttributes { Font = font };
                CGSize frameSize = nsString.GetSizeUsingAttributes(attribs);
                if(frameSize.Width < label.Bounds.Width)
                {
                    label.Font = font;
                    break;
                }
            }

            CGSize sizeToDisplay = new CGSize(label.Bounds.Width, float.MaxValue);
            nsString = new NSString(label.Text);
            for(; size >= 10; --size)
            {
                var font = label.Font.WithSize(size);
                CGSize frameSize = UIStringDrawing.StringSize(nsString, font, sizeToDisplay, UILineBreakMode.TailTruncation);

                if(frameSize.Width <= label.Bounds.Width && frameSize.Height <= label.Bounds.Height)
                {
                    label.Font = font;
                    break;
                }
            }
        }
    }
}