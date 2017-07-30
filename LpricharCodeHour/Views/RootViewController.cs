using System;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using LpricharCodeHour.Controls;
using LpricharCodeHour.Utils;

namespace LpricharCodeHour.Views
{
    public class RootView : UIView
    {
        private UILabel _label;
        private CounterView _counterView;

        public RootView()
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

        public async void StartAnimation()
        {
            try
            {
                while (true)
                {
                    await Task.WhenAll(
                        _counterView.Pulse(),
                        Task.Delay(2000)
                        );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in animation " + ex);
            }
        }
    }

    [Register("RootViewController")]
    public class RootViewController : UIViewController
    {
        private RootView _rootView;

        public override void ViewDidLoad()
        {
            _rootView = new RootView();
            View = _rootView;

            base.ViewDidLoad();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _rootView.StartAnimation();
        }
    }
}