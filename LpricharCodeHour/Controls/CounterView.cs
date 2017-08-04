using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using LpricharCodeHour.Utils;
using UIKit;

namespace LpricharCodeHour.Controls
{
    public sealed class CounterView : UIView
    {
        private enum CounterState
        {
            Circle,
            Square
        }

        private readonly CAShapeLayer _layer;
        private CounterState _counterState;

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

        public override async void Draw(CGRect rect)
        {
            base.Draw(rect);

            if (_counterState == CounterState.Circle)
            {
                using (var oval = UIBezierPath.FromOval(rect))
                {
                    _layer.Path = oval.CGPath;
                }
            }
            else if (_counterState == CounterState.Square)
            {
                var duration = 1f;
                using (var path = GetSquarePath(rect))
                {
                    await AnimatePathTo(_layer, duration, path);
                }
            }
        }

        private async Task AnimatePathTo(CAShapeLayer shapeLayer, float duration, UIBezierPath toBezierPath)
        {
            var fromCgPath = FromObject(shapeLayer.Path);
            var toCgPath = FromObject(toBezierPath.CGPath);

            shapeLayer.Path = toBezierPath.CGPath;
            await AnimationUtils.BasicAnimationAsync(shapeLayer, "path", duration, fromCgPath, toCgPath, CAMediaTimingFunction.EaseInEaseOut);
        }

        private UIBezierPath GetSquarePath(CGRect containerRect)
        {
            return UIBezierPath.FromRect(containerRect);
        }

        public void ResetToCircle()
        {
            _counterState = CounterState.Circle;
            // aka goto Draw()
            SetNeedsDisplay();
        }

        public void AnimateToSquare()
        {
            _layer.StrokeEnd = 1f;
            _counterState = CounterState.Square;
            // aka goto Draw()
            SetNeedsDisplay();
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