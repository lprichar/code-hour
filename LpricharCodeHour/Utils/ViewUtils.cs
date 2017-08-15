using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LpricharCodeHour.Utils
{
    public static class ViewUtils
    {
        public static UILabel AddLabel(this UIView parent, string text, nfloat fontSize, UIColor textColor = null)
        {
            var label = new UILabel
            {
                Text = text,
                Lines = 0,
                TextColor = textColor ?? UIColor.White,
            };
            label.Font = label.Font.WithSize(fontSize);
            parent.AddSubview(label);
            return label;
        }
    }
}