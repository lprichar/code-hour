using System;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using Foundation;
using LpricharCodeHour.Controls;
using LpricharCodeHour.Utils;

namespace LpricharCodeHour.Views
{
    public class RootView : UIView
    {
        private UILabel _initiatingLabel;
        private UIView _codeHourFrame;
        private CounterView _counterView;
        private UILabel _counterLabel;
        private UIImageView _watchImageView;
        private UIImage _watch;
        private UILabel _lpricharLabel;
        private UILabel _codeHourLabel;
        private BlinkySquareView _row1Cursor;
        private BlinkySquareView _row2Cursor;

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
            _codeHourFrame = AddView(this);
            _initiatingLabel = AddLabel(this, "", 20);
            _counterLabel = AddLabel(this, "", 40);
            _watch = UIImage.FromBundle("Watch");
            _watchImageView = AddImageView(this, _watch);
            _counterView = AddCounterView(this);
            AddLpricharLabel();
            _codeHourLabel = AddLabel(this, "code hour", 60);
            _codeHourLabel.Alpha = 0f;
            _row1Cursor = AddBlinkySquareView(this);
            _row2Cursor = AddBlinkySquareView(this);
        }

        private static UIView AddView(UIView parent)
        {
            var view = new UIView();
            parent.AddSubview(view);
            return view;
        }


        private static BlinkySquareView AddBlinkySquareView(UIView parent)
        {
            var blinkySquareView = new BlinkySquareView();
            parent.AddSubview(blinkySquareView);
            return blinkySquareView;
        }


        private void AddLpricharLabel()
        {
            _lpricharLabel = AddLabel(this, "lprichar", 75);
            _lpricharLabel.Alpha = 0f;
            _lpricharLabel.TextColor = UIColor.FromRGB(213, 43, 47);
        }

        private static UIImageView AddImageView(UIView parent, UIImage image)
        {
            var imageView = new UIImageView
            {
                Image = image,
                Alpha = 0,
                BackgroundColor = UIColor.Clear
            };
            parent.AddSubview(imageView);
            return imageView;
        }


        private static CounterView AddCounterView(UIView parent)
        {
            var counterView = new CounterView();
            parent.AddSubview(counterView);
            return counterView;
        }

        private static UILabel AddLabel(UIView parent, string text, nfloat fontSize)
        {
            var label = new UILabel
            {
                Text = text,
                Lines = 0,
                TextColor = UIColor.White,
            };
            label.Font = label.Font.WithSize(fontSize);
            parent.AddSubview(label);
            return label;
        }

        private void ConstrainLayout()
        {
            this.ConstrainLayout(() =>
                _initiatingLabel.Frame.Top == Frame.Top + 50
                && _initiatingLabel.Frame.Left == Frame.Left + 10

                && _row1Cursor.Frame.Left == _initiatingLabel.Frame.Right + 1
                && _row1Cursor.Frame.Top == _initiatingLabel.Frame.Top
                && _row1Cursor.Frame.Bottom == _initiatingLabel.Frame.Bottom
                && _row1Cursor.Frame.Width == 10

                && _row2Cursor.Frame.Left == _initiatingLabel.Frame.Left
                && _row2Cursor.Frame.Top == _initiatingLabel.Frame.Bottom + 10
                && _row2Cursor.Frame.Height == _initiatingLabel.Frame.Height
                && _row2Cursor.Frame.Width == _row1Cursor.Frame.Width

                && _counterView.Frame.GetCenterX() == Frame.GetCenterX()
                && _counterView.Frame.GetCenterY() == Frame.GetCenterY()
                && _counterView.Frame.Height == 100
                && _counterView.Frame.Width == 100

                && _counterLabel.Frame.GetCenterX() == _counterView.Frame.GetCenterX()
                && _counterLabel.Frame.GetCenterY() == _counterView.Frame.GetCenterY()

                && _watchImageView.Frame.GetCenterY() == Frame.GetCenterY()
                && _watchImageView.Frame.GetCenterX() == Frame.GetCenterX() + 200

                && _lpricharLabel.Frame.Right == Frame.GetCenterX() + 40
                && _lpricharLabel.Frame.Bottom == Frame.GetCenterY() - 15

                && _codeHourLabel.Frame.Top == Frame.GetCenterY() + 15
                && _codeHourLabel.Frame.Right == _lpricharLabel.Frame.Right

                && _codeHourFrame.Frame.Top == _lpricharLabel.Frame.Top - 60
                && _codeHourFrame.Frame.Left == _codeHourLabel.Frame.Left - 30
                && _codeHourFrame.Frame.Right == _lpricharLabel.Frame.Right + 280
                && _codeHourFrame.Frame.Bottom == _codeHourLabel.Frame.Bottom + 65
            );
        }

        private async Task StartCountdownAnim()
        {
            for (int i = 3; i >= 0; i--)
            {
                var isOdd = i % 2 == 1;

                FadeShow(i);

                if (isOdd)
                {
                    var pulseDuration = 1.8f;
                    _counterView.Pulse(pulseDuration).FireAndForget();
                }
                await Task.Delay(1000);
            }
        }

        private async Task FadeShow(int i)
        {
            _counterLabel.Text = i.ToString();
            await UIView.AnimateAsync(.1f, () => _counterLabel.Alpha = 1);
            await Task.Delay(800);
            await UIView.AnimateAsync(.1f, () => _counterLabel.Alpha = 0);
        }

        private bool _animationInProgress = false;

        public override async void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            await AnimateOnce();
        }

        private async Task AnimateOnce()
        {
            if (_animationInProgress) return;
            _animationInProgress = true;
            try
            {
                _row1Cursor.Stop();
                await TypeInitiate();
                MakeRowActive(1);
                HideAfter(1500, _row2Cursor, _initiatingLabel).FireAndForget();
                await Task.Delay(500);
                await StartCountdownAnim();
                //_counterView.Frame = _codeHourFrame.Frame;
                //_counterView.AnimateToSquare();
                await ZoomWatch();
                await ShowLpricharShowText();
                await Task.Delay(5000);
                ResetEverything();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in animation " + ex);
            }
            finally
            {
                _animationInProgress = false;
            }
        }

        private async Task HideAfter(int duration, params UIView[] views)
        {
            await Task.Delay(duration);
            foreach (var view in views)
            {
                view.Alpha = 1;
                Animate(1, () =>
                {
                    view.Alpha = 0;
                });
            }
        }

        private async Task ShowLpricharShowText()
        {
            _lpricharLabel.Alpha = 0;
            _codeHourLabel.Alpha = 0;
            await AnimateAsync(.5f, () =>
            {
                _lpricharLabel.Alpha = 1;
                _codeHourLabel.Alpha = 1;
            });
        }

        private async Task ZoomWatch()
        {
            _watchImageView.Alpha = 0;
            var zoomBackTo = .2f;
            await AnimateNotifyAsync(.6f, 0, UIViewAnimationOptions.CurveEaseOut, () =>
            {
                _watchImageView.Alpha = 1;
                _watchImageView.Transform = CGAffineTransform.MakeScale(zoomBackTo, zoomBackTo);
            });
        }

        public void ResetEverything()
        {
            _counterLabel.Text = "";
            _counterView.ResetToCircle();
            _initiatingLabel.Text = "Lees-iPadPro$ ";
            _initiatingLabel.Alpha = 1;
            _watchImageView.Alpha = 0;
            _watchImageView.Transform = CGAffineTransform.MakeScale(6f, 6f);
            _lpricharLabel.Alpha = 0;
            _codeHourLabel.Alpha = 0;
            MakeRowActive(0);
            _row1Cursor.Start();
            _row2Cursor.Alpha = 1;
        }

        private void MakeRowActive(int i)
        {
            _row1Cursor.Hidden = i != 0;
            _row2Cursor.Hidden = i == 0;
        }

        private async Task TypeInitiate()
        {
            var duration = 200;
            await AppendInitiateCharacter("i", duration / 2);
            await AppendInitiateCharacter("n", duration / 2);
            await AppendInitiateCharacter("i", duration / 2);
            await AppendInitiateCharacter("t", duration);
            await AppendInitiateCharacter("iate ", duration * 2);
            await AppendInitiateCharacter("'", duration);
            await AppendInitiateCharacter("l", duration);
            await AppendInitiateCharacter("p", duration);
            await AppendInitiateCharacter("r", duration);
            await AppendInitiateCharacter("i", duration);
            await AppendInitiateCharacter("char code hour'", duration);
            //initiate 'lprichar code hour'
        }

        private async Task AppendInitiateCharacter(string str, int duration)
        {
            _initiatingLabel.Text += str;
            await Task.Delay(duration);
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
            _rootView.ResetEverything();
        }
    }
}