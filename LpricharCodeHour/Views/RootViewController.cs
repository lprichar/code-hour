using System;
using UIKit;
using Foundation;
using LpricharCodeHour.Utils;

namespace LpricharCodeHour.Views
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        private UILabel _label;

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
            _label = AddLabel(this, "lprichar" + Environment.NewLine + "Code Hour");
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
                _label.Frame.GetCenterX() == Frame.GetCenterX()
                && _label.Frame.GetCenterY() == Frame.GetCenterY()
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