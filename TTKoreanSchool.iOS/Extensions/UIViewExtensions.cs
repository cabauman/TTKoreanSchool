using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace TTKoreanSchool.iOS.Extensions
{
    public static class UIViewExtensions
    {
        public static void FlipHorizontaly(this UIView view, bool isIn, double duration = 0.3, Action onFinished = null)
        {
            var m34 = (nfloat)(-1 * 0.0003);

            var minAlpha = (nfloat)0.0f;
            var maxAlpha = (nfloat)1.0f;

            view.Alpha = (nfloat)1.0;

            var minTransform = CATransform3D.Identity;
            minTransform.m34 = m34;
            minTransform = minTransform.Rotate((nfloat)((isIn ? 1 : -1) * Math.PI * 0.5), 0.0f, 1.0f, 0.0f);
            var maxTransform = CATransform3D.Identity;
            maxTransform.m34 = m34;

            view.Alpha = isIn ? minAlpha : maxAlpha;
            view.Layer.Transform = isIn ? minTransform : maxTransform;
            UIView.Animate(
                duration,
                0,
                UIViewAnimationOptions.CurveEaseInOut,
                () =>
                {
                    view.Layer.AnchorPoint = new CGPoint(0.5f, 0.5f);
                    view.Layer.Transform = isIn ? maxTransform : minTransform;
                    view.Alpha = isIn ? maxAlpha : minAlpha;
                },
                onFinished
            );
        }
    }
}