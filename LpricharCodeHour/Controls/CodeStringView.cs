using System;
using System.Linq;
using Foundation;
using LpricharCodeHour.Utils;
using UIKit;

namespace LpricharCodeHour.Controls
{
    public class Hi
    {
        
    }

    public class CodeStringView : UIView
    {
        private UILabel _uiLabel;

        public CodeStringView()
        {
            AddViews();
            ConstrainLayout();
        }

        private void ConstrainLayout()
        {
            this.ConstrainLayout(() =>
                _uiLabel.Frame.Left == Frame.Left
                && _uiLabel.Frame.Right == Frame.Right
                && _uiLabel.Frame.Bottom == Frame.Bottom
            );
        }

        private void AddViews()
        {
            var baseStr = "if (_inProgress) return true; try { _row.Stop(); } catch (Exception ex) { Console.Write(\"Error in animation \" + ex); } this.ConstrainLayout(() => view.Frame.Top == Frame.Top + 50);";
            var codeText = baseStr.ToCharArray().Take(26);
            var str = string.Join(Environment.NewLine, codeText);
            _uiLabel = AddLabel(this, str);
        }

        private static UILabel AddLabel(UIView parent, string str)
        {
            var label = new UILabel();
            parent.AddSubview(label);

            var uiColors = new []
            {
                UIColor.FromRGB(29, 112, 37),
                UIColor.FromRGB(33, 98, 42),
                UIColor.FromRGB(17, 155, 53),
                UIColor.FromRGB(68, 144, 72),
                UIColor.FromRGB(14, 154, 46),
                UIColor.FromRGB(25, 178, 58),
                UIColor.FromRGB(47, 151, 70),
                UIColor.FromRGB(40, 164, 58),
                UIColor.FromRGB(71, 172, 90),
                UIColor.FromRGB(81, 193, 104),
                UIColor.FromRGB(43, 171, 74),
                UIColor.FromRGB(63, 193, 94),
                UIColor.FromRGB(61, 174, 85),
                UIColor.FromRGB(27, 161, 50),
                UIColor.FromRGB(100, 212, 115),
                UIColor.FromRGB(86, 189, 97),
                UIColor.FromRGB(95, 213, 101),
                UIColor.FromRGB(91, 220, 124),
                UIColor.FromRGB(100, 209, 112),
                UIColor.FromRGB(100, 226, 124),
                UIColor.FromRGB(111, 226, 128),
                UIColor.FromRGB(117, 239, 122),
                UIColor.FromRGB(97, 225, 122),
                UIColor.FromRGB(100, 218, 113),
                UIColor.FromRGB(121, 226, 131),
                UIColor.FromRGB(146, 246, 162),
            };

            var attributes = uiColors
                .Select(c => new UIStringAttributes {ForegroundColor = c})
                .ToArray();

            var attributedString = new NSMutableAttributedString(str);

            var colorCount = uiColors.Length;

            for (int i = 0; i < str.Length; i++)
            {
                var percentCharInString = (float)i / str.Length;
                var attrToUse = (int) Math.Ceiling(percentCharInString * (colorCount - 1));
                var attribute = attributes[attrToUse];
                attributedString.SetAttributes(attribute, new NSRange(i, 1));
            }

            label.AttributedText = attributedString;
            label.Lines = 0;
            label.LineBreakMode = UILineBreakMode.Clip;
            label.Font = UIFont.FromName(label.Font.Name, 26f);
            label.TextAlignment = UITextAlignment.Center;

            return label;
        }

    }
}