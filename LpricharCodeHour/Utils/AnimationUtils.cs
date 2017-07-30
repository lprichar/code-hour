using System;
using System.Linq;
using System.Threading.Tasks;
using CoreAnimation;
using Foundation;
using UIKit;

namespace LpricharCodeHour.Utils
{
    public static class AnimationUtils
    {
        public static Task<bool> BasicAnimationAsync(
            CAShapeLayer layer,
            string path,
            float duration,
            float from,
            float to,
            NSString timingFunction = null,
            string animationName = null
        )
        {
            var nsFrom = new NSNumber(@from);
            var nsTo = new NSNumber(to);
            return BasicAnimationAsync(layer, path, duration, nsFrom, nsTo, timingFunction, animationName);
        }

        public static async Task<bool> BasicAnimationAsync(
            CAShapeLayer layer,
            string path,
            float duration,
            NSObject from,
            NSObject to,
            NSString timingFunction = null,
            string animationName = null
        )
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            using (var drawAnimation = CABasicAnimation.FromKeyPath(path))
            {
                drawAnimation.Duration = duration;
                drawAnimation.RepeatCount = 1;

                drawAnimation.From = @from;
                drawAnimation.To = to;

                if (timingFunction != null)
                {
                    drawAnimation.TimingFunction = CAMediaTimingFunction.FromName(timingFunction);
                }

                drawAnimation.AnimationStopped += (sender, args) => { taskCompletionSource.TrySetResult(args.Finished); };

                SetShapeLayerEndState(layer, drawAnimation);
                var key = animationName ?? Guid.NewGuid().ToString();
                layer.AddAnimation(drawAnimation, key);

                return await taskCompletionSource.Task;
            }
        }

        public static void SetShapeLayerEndState(CAShapeLayer shapeLayer, CABasicAnimation drawAnimation)
        {
            if (drawAnimation.KeyPath == "strokeEnd")
            {
                var to = ((NSNumber)drawAnimation.To).NFloatValue;
                shapeLayer.StrokeEnd = to;
                if (to == 1)
                {
                    shapeLayer.Opacity = 1f;
                }
            }
            if (drawAnimation.KeyPath == "strokeStart")
            {
                var to = ((NSNumber)drawAnimation.To).NFloatValue;
                shapeLayer.StrokeStart = to;
            }
            if (drawAnimation.KeyPath == "opacity")
            {
                var to = ((NSNumber)drawAnimation.To).NFloatValue;
                shapeLayer.Opacity = (float)to;
            }
        }

        public static Task AnimateVisibilityOfAllViews(UIView[] allLabels, bool visible, float duration)
        {
            var allAnimations = allLabels.Select(label => { return UIView.AnimateNotifyAsync(duration, () => { label.Alpha = visible ? 1 : 0; }); });
            return Task.WhenAny(allAnimations);
        }
    }
}