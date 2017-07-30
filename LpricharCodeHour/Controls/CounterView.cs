using CoreAnimation;
using CoreGraphics;
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
                LineWidth = 2f
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
    }
}