using System;
using UIKit;
using Foundation;
using LpricharCodeHour.Controls;
using LpricharCodeHour.Utils;

namespace LpricharCodeHour.Views
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        private UILabel _label;
        private CounterView _counterView;

        public UniversalView()
        {
            Initialize();
            AddViews();
            ConstrainLayout();
        }

        void Initialize()
        {
            var color = 0.1450980392156863f;
            BackgroundColor = UIColor.FromRGB(color, color, color);
        }

        private void AddViews()
        {
            _label = AddLabel(this, "initiating lprichar code hour" + Environment.NewLine + "t minus");

            _counterView = AddCounterView(this);
        }

        private static CounterView AddCounterView(UIView parent)
        {
            var counterView = new CounterView();
            parent.AddSubview(counterView);
            return counterView;
        }


        private static UILabel AddLabel(UIView parent, string text)
        {
            var label = new UILabel
            {
                Text = text,
                Lines = 0,
                TextColor = UIColor.White
            };
            parent.AddSubview(label);
            return label;
        }

        private void ConstrainLayout()
        {
            this.ConstrainLayout(() =>
                _label.Frame.Top == Frame.Top + 50
                && _label.Frame.Left == Frame.Left + 10

                && _counterView.Frame.GetCenterX() == Frame.GetCenterX()
                && _counterView.Frame.GetCenterY() == Frame.GetCenterY()
                && _counterView.Frame.Height == 100
                && _counterView.Frame.Width == 100
            );
        }
    }

    [Register("RootViewController")]
    public class RootViewController : UIViewController
    {
        public RootViewController()
        {
        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();
        }
    }
}