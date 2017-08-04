using System;
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
            _uiLabel = AddLabel(this);
        }


        private static UILabel AddLabel(UIView parent)
        {
            var label = new UILabel();
            parent.AddSubview(label);
            char[] codeText = "if (_animationInProgress) return; try { _row1Cursor.Stop(); } catch (Exception ex) { Console.WriteLine(\"Error in animation \" + ex); } ".ToCharArray();
            var str = string.Join(Environment.NewLine, codeText);
            label.Text = str;
            label.Lines = 0;
            label.TextColor = UIColor.FromRGB(115, 238, 138);
            label.LineBreakMode = UILineBreakMode.Clip;

            return label;
        }

    }
}