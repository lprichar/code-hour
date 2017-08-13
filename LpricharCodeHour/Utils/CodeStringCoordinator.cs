using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LpricharCodeHour.Controls;
using UIKit;

namespace LpricharCodeHour.Utils
{
    public class CodeStringCoordinator
    {
        class CodeStringMeta
        {
            public CodeStringMeta(CodeStringView codeStringView, int column)
            {
                CodeStringView = codeStringView;
                Column = column;
            }

            public CodeStringView CodeStringView { get; }
            int Column { get; }
        }

        private readonly List<CodeStringMeta> _allCodeStrings = new List<CodeStringMeta>();
        public nfloat TextWidth { private get; set; }
        public nfloat TextHeight { private get; set; }
        public CodeStringView CenterCodeString { private get; set; }

        public CodeStringView AddToRightOf(int column, CodeStringView relativeView, UIView parent)
        {
            var codeStringView = AddCodeStringView(parent);
            var pixelsBetweenRows = GetPixelsBetweenRows();
            parent.ConstrainLayout(() =>
                codeStringView.Frame.Left == relativeView.Frame.Right + pixelsBetweenRows
            );

            return ConstrainAndAddToMeta(column, parent, codeStringView);
        }

        public CodeStringView AddToLeftOf(int column, CodeStringView relativeView, UIView parent)
        {
            var codeStringView = AddCodeStringView(parent);
            var pixelsBetweenRows = GetPixelsBetweenRows();
            parent.ConstrainLayout(() =>
                codeStringView.Frame.Right == relativeView.Frame.Left - pixelsBetweenRows
            );

            return ConstrainAndAddToMeta(column, parent, codeStringView);
        }

        private nfloat GetPixelsBetweenRows()
        {
            return CodeStringsScene.CodeStringMargin - TextWidth;
        }

        private CodeStringView ConstrainAndAddToMeta(int column, UIView parent, CodeStringView codeStringView)
        {
            AddWidthConstraint(parent, codeStringView);
            AddHeightConstraint(parent, codeStringView);
            AddBottomConstraint(parent, codeStringView, CenterCodeString, column);
            var codeStringMeta = new CodeStringMeta(codeStringView, column);
            _allCodeStrings.Add(codeStringMeta);
            return codeStringView;
        }

        private void AddHeightConstraint(UIView parent, CodeStringView codeStringView)
        {
            parent.AddConstraint(NSLayoutConstraint.Create(codeStringView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, TextHeight));
        }

        private void AddWidthConstraint(UIView parent, CodeStringView codeStringView)
        {
            parent.AddConstraint(NSLayoutConstraint.Create(codeStringView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, TextWidth * 3));
        }

        static readonly Random DistanceFromCenterRandom = new Random(42);

        private static float GetDistanceFromCenter(int column)
        {
            if (column == 10) return 500;
            if (column == 3) return 600;
            if (column == -5) return 620;
            if (column == 2) return 700;
            return DistanceFromCenterRandom.Next(800, CodeStringsScene.MaxDistanceFromCenter);
        }

        private static void AddBottomConstraint(UIView parent, CodeStringView codeStringView, UIView centerCodeString, int column)
        {
            var distanceFromCenter = GetDistanceFromCenter(column);
            var bottomConstraint = NSLayoutConstraint.Create(
                codeStringView, NSLayoutAttribute.Bottom,
                NSLayoutRelation.Equal,
                centerCodeString, NSLayoutAttribute.Bottom,
                1, -distanceFromCenter);
            parent.AddConstraint(bottomConstraint);
        }

        public async Task StartAnimations(int delayInMs)
        {
            await Task.Delay(delayInMs);
            _allCodeStrings.ForEach(cs => cs.CodeStringView.StartAnimation());
        }

        public void StopAnimations()
        {
            _allCodeStrings.ForEach(cs => cs.CodeStringView.StopAnimation());
        }

        public static CodeStringView AddCodeStringView(UIView parent)
        {
            var codeStringView = new CodeStringView();
            parent.AddSubview(codeStringView);
            return codeStringView;
        }
    }
}