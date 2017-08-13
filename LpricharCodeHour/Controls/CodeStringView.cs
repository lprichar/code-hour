using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using LpricharCodeHour.Utils;
using UIKit;

namespace LpricharCodeHour.Controls
{
    public sealed class CodeStringView : UIView
    {
        private UILabel[] _uiLabels;
        private char[] _codeText;
        private const float CharMargin = 2;
        private const string BaseStr = "if(_inProgress)returntrue;try{_row.Stop();}catch(Exceptionex){Console.Write(\"Errorinanimation\"+ex);}this.ConstrainLayout(()=>view.Frame.Top==Frame.Top+50);";
        private nfloat? _textHeight;
        private nfloat? _textWidth;

        public CodeStringView()
        {
            BackgroundColor = UIColor.Clear;
            AddViews();
            ConstrainLayout();
        }

        private void ConstrainLayout()
        {
            var first = _uiLabels[0];

            this.ConstrainLayout(() =>
                first.Frame.GetCenterX() == Frame.GetCenterX()
                && first.Frame.Bottom == Frame.Bottom - 20
            );

            var previous = first;

            for (int i = 1; i < _uiLabels.Length; i++)
            {
                var current = _uiLabels[i];
                this.ConstrainLayout(() =>
                    current.Frame.GetCenterX() == Frame.GetCenterX()
                    && current.Frame.Bottom == previous.Frame.Top + CharMargin
                );
                previous = current;
            }
        }

        private static readonly Random Rnd = new Random(47);
        private bool _animationRunning = false;

        private static readonly UIColor[] UiColors = {
            UIColor.FromRGB(146, 246, 162),
            UIColor.FromRGB(121, 226, 131),
            UIColor.FromRGB(100, 218, 113),
            UIColor.FromRGB(97, 225, 122),
            UIColor.FromRGB(117, 239, 122),
            UIColor.FromRGB(111, 226, 128),
            UIColor.FromRGB(100, 226, 124),
            UIColor.FromRGB(100, 209, 112),
            UIColor.FromRGB(91, 220, 124),
            UIColor.FromRGB(95, 213, 101),
            UIColor.FromRGB(86, 189, 97),
            UIColor.FromRGB(100, 212, 115),
            UIColor.FromRGB(27, 161, 50),
            UIColor.FromRGB(61, 174, 85),
            UIColor.FromRGB(63, 193, 94),
            UIColor.FromRGB(43, 171, 74),
            UIColor.FromRGB(81, 193, 104),
            UIColor.FromRGB(71, 172, 90),
            UIColor.FromRGB(40, 164, 58),
            UIColor.FromRGB(47, 151, 70),
            UIColor.FromRGB(25, 178, 58),
            UIColor.FromRGB(14, 154, 46),
            UIColor.FromRGB(68, 144, 72),
            UIColor.FromRGB(17, 155, 53),
            UIColor.FromRGB(33, 98, 42),
            UIColor.FromRGB(29, 112, 37),
        };

        private void AddViews()
        {
            _codeText = GetCodeString();
            _uiLabels = MakeUiLabels(_codeText).ToArray();
            SetLabelColors(_codeText);
        }

        private void SetLabelColors(char[] codeText)
        {
            var colorCount = UiColors.Length;

            int i = 0;
            foreach (var uiLabel in _uiLabels)
            {
                var percentCharInString = (float)i / codeText.Length;
                var colorToUse = (int)Math.Ceiling(percentCharInString * (colorCount - 1));
                var color = UiColors[colorToUse];
                uiLabel.TextColor = color;

                i++;
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            nfloat red, green, blue, alpha;
            BackgroundColor.GetRGBA(out red, out green, out blue, out alpha);

            nfloat[] colors1 = {
                .25f, .74f, .39f, 1f,
                red, green, blue, 0,
            };
            nfloat[] colors2 = {
                .10f, .46f, .18f, 1f,
                red, green, blue, 0,
            };
            nfloat[] colors3 = {
                .08f, .34f, .09f, 1f,
                red, green, blue, 0,
            };

            using (var context = UIGraphics.GetCurrentContext())
            using (var gradient1 = new CGGradient(CGColorSpace.CreateDeviceRGB(), colors1))
            using (var gradient2 = new CGGradient(CGColorSpace.CreateDeviceRGB(), colors2))
            using (var gradient3 = new CGGradient(CGColorSpace.CreateDeviceRGB(), colors3))
            {
                //var center = new CGPoint(rect.GetCenterX(), rect.Height - 20);
                //var radius1 = 40;

                /*
                 * a => xx
                 * b => yx
                 * c => xy
                 * d => yy
                 * tx => x0
                 * ty => y0 
                 */

                DrawRadialGradient(rect, context, gradient1, -35);
                DrawRadialGradient(rect, context, gradient2, -65);
                for (int i = 1; i < 6; i++)
                {
                    var delta = 65 + (30 * i);
                    DrawRadialGradient(rect, context, gradient3, -delta);
                }
            }
        }

        private static void DrawRadialGradient(CGRect rect, CGContext context, CGGradient gradient, int yOffset)
        {
            CGAffineTransform scaleT = CGAffineTransform.MakeScale(1, 2f);
            CGAffineTransform invScaleT = CGAffineTransform.CGAffineTransformInvert(scaleT);
            //// Extract the Sx and Sy elements from the inverse matrix
            //// (See the Quartz documentation for the math behind the matrices)
            CGPoint invS = new CGPoint(invScaleT.xx, invScaleT.yy);

            //// Transform center and radius of gradient with the inverse
            CGPoint center = new CGPoint((rect.Size.Width * .5f) * invS.X, (rect.Size.Height + yOffset) * invS.Y);
            var radius = (rect.Size.Width * .4f) * invS.X;
            context.ScaleCTM(scaleT.xx, scaleT.yy);
            context.DrawRadialGradient(gradient, center, 0, center, radius, CGGradientDrawingOptions.DrawsBeforeStartLocation);
            context.ScaleCTM(invScaleT.xx, invScaleT.yy);
        }

        private IEnumerable<UILabel> MakeUiLabels(char[] codeText)
        {
            foreach (var charInStr in codeText)
            {
                yield return AddLabel(this, charInStr.ToString());
            }
        }

        private static char[] GetCodeString()
        {
            const int charsInString = 26;
            var startChar = Rnd.Next(0, BaseStr.Length - charsInString - 1);
            var codeText = BaseStr.ToCharArray().Skip(startChar).Take(charsInString);
            return codeText.ToArray();
        }

        public async void StartAnimation()
        {
            if (_animationRunning) return;
            _animationRunning = true;
            while (_animationRunning)
            {
                var charToReplace = Rnd.Next(0, _codeText.Length - 1);
                var newChar = BaseStr[Rnd.Next(0, BaseStr.Length - 1)];
                //Console.WriteLine($"Replacing char at #{charToReplace} which is '{existingChar}' with '{newChar}'");
                _uiLabels[charToReplace].Text = newChar.ToString();
                await Task.Delay(40);
            }
        }

        public void StopAnimation()
        {
            _animationRunning = false;
        }

        private static UILabel AddLabel(UIView parent, string str)
        {

            var label = new UILabel();
            parent.AddSubview(label);

            label.Text = str;
            label.LineBreakMode = UILineBreakMode.Clip;
            label.Font = UIFont.FromName(label.Font.Name, 26f);
            label.TextAlignment = UITextAlignment.Center;

            return label;
        }

        public nfloat GetTextHeight()
        {
            return _textHeight ?? (_textHeight = CalculateTextHeight()).Value;
        }

        private nfloat CalculateTextHeight()
        {
            var firstLabel = _uiLabels[0];
            firstLabel.SizeToFit();
            var charHeight = firstLabel.Frame.Height;
            return (charHeight + CharMargin) * _codeText.Length;
        }

        public nfloat GetTextWidth()
        {
            return _textWidth ?? (_textWidth = CalculateTextWidth()).Value;
        }

        private nfloat CalculateTextWidth()
        {
            var firstLabel = _uiLabels[0];
            firstLabel.SizeToFit();
            return firstLabel.Frame.Width;
        }
    }
}