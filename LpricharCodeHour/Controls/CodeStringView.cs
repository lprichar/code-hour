using System;
using System.Linq;
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
                _uiLabel.Frame.Top == Frame.Top
                && _uiLabel.Frame.Left == Frame.Left
                && _uiLabel.Frame.Right == Frame.Right
                && _uiLabel.Frame.Bottom == Frame.Bottom
            );
        }

        private void AddViews()
        {
            var baseStr = "if (_inProgress) return true; try { _row.Stop(); } catch (Exception ex) { Console.Write(\"Error in animation \" + ex); } this.ConstrainLayout(() => view.Frame.Top == Frame.Top + 50);";
            var codeText = baseStr.ToCharArray().Take(16);
            var str = string.Join(Environment.NewLine, codeText);
            _uiLabel = AddLabel(this, str);
        }

        private static UILabel AddLabel(UIView parent, string str)
        {
            var label = new UILabel();
            parent.AddSubview(label);
            label.Text = str;
            label.Lines = 0;
            label.TextColor = UIColor.FromRGB(115, 238, 138);
            label.LineBreakMode = UILineBreakMode.Clip;
            label.Font = UIFont.FromName(label.Font.Name, 26f);
            label.TextAlignment = UITextAlignment.Center;

            return label;
        }

    }
}