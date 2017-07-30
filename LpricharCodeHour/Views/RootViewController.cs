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
        private UILabel _initiatingLabel;
        private CounterView _counterView;
        private UILabel _counterLabel;

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
            _initiatingLabel = AddLabel(this, "initiating lprichar code hour" + Environment.NewLine + "t minus");
            _counterLabel = AddLabel(this, "");

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
                _initiatingLabel.Frame.Top == Frame.Top + 50
                && _initiatingLabel.Frame.Left == Frame.Left + 10

                && _counterView.Frame.GetCenterX() == Frame.GetCenterX()
                && _counterView.Frame.GetCenterY() == Frame.GetCenterY()
                && _counterView.Frame.Height == 100
                && _counterView.Frame.Width == 100

                && _counterLabel.Frame.GetCenterX() == _counterView.Frame.GetCenterX()
                && _counterLabel.Frame.GetCenterY() == _counterView.Frame.GetCenterY()
            );
        }

        private async Task StartCountdownAnim()
        {
            for (int i = 9; i >= 0; i--)
            {
                var isOdd = i % 2 == 1;

                _counterLabel.Text = i.ToString();

                if (isOdd)
                {
                    var pulseDuration = 1.8f;
                    _counterView.Pulse(pulseDuration).FireAndForget();
                }
                await Task.Delay(1000);
            }
        }

        public async void StartAnimation()
        {
            try
            {
                while (true)
                {
                    await StartCountdownAnim();
                    _counterLabel.Text = "";
                    await Task.Delay(3000);
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