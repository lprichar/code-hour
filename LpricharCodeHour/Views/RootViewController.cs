using UIKit;
using Foundation;
using LpricharCodeHour.Utils;

namespace LpricharCodeHour.Views
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("RootViewController")]
    public class RootViewController : UIViewController
    {
        private UILabel _label;

        public RootViewController()
        {
        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();

            AddViews();
            ConstrainLayout();
        }

        private void AddViews()
        {
            _label = AddLabel(View, "The lprichar Code Hour");
        }

        private static UILabel AddLabel(UIView parent, string text)
        {
            var label = new UILabel
            {
                Text = text
            };
            parent.AddSubview(label);
            return label;
        }

        private void ConstrainLayout()
        {
            View.ConstrainLayout(() => 
                _label.Frame.GetCenterX() == View.Frame.GetCenterX()
                && _label.Frame.GetCenterY() == View.Frame.GetCenterY()
            );
        }
    }
}