using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using LpricharCodeHour.Utils;
using UIKit;

namespace LpricharCodeHour.Controls
{
    public sealed class CounterView : UIView
    {
        private readonly CAShapeLayer _layer;

        public CounterView()
        {
            BackgroundColor = UIColor.Clear;
            _layer = MakeBackgroundShapeLayer();
        }

        private CAShapeLayer MakeBackgroundShapeLayer()
        {
            var shapeLayer = new CAShapeLayer
            {
                FillColor = UIColor.Clear.CGColor,
                StrokeColor = UIColor.White.CGColor,
                LineWidth = 2f,
                StrokeEnd = 0
            };
            Layer.AddSublayer(shapeLayer);
            return shapeLayer;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            using (var oval = UIBezierPath.FromOval(rect))
            {
                var cgpath = oval.CGPath;
                _layer.Path = cgpath;
            }
        }

        public async Task Pulse(float duration)
        {
            var halfDuration = duration / 2;
            await AnimationUtils.BasicAnimationAsync(_layer, "strokeEnd", halfDuration, 0f, 1f, CAMediaTimingFunction.EaseInEaseOut);
            await AnimationUtils.BasicAnimationAsync(_layer, "strokeStart", halfDuration, 0f, 1f, CAMediaTimingFunction.EaseInEaseOut);
            await ResetLayer();
        }

        /// <summary>
        /// Resetting the strokeEnd and strokeStart is a little tricky since it will animate
        /// to the values you set it to if you aren't careful and that looks bad
        /// </summary>
        /// <returns></returns>
        private async Task ResetLayer()
        {
            _layer.Opacity = 0f;
            await AnimationUtils.BasicAnimationAsync(_layer, "strokeEnd", .01f, 1f, 0f, CAMediaTimingFunction.Linear);
            await AnimationUtils.BasicAnimationAsync(_layer, "strokeStart", .01f, 1f, 0f, CAMediaTimingFunction.Linear);
            _layer.Opacity = 1f;
        }
    }
}