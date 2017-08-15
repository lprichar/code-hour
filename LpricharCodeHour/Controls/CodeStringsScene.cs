using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using LpricharCodeHour.Utils;
using UIKit;

namespace LpricharCodeHour.Controls
{
    public sealed class CodeStringsScene : UIView
    {
        public const int MaxDistanceFromCenter = 2500;
        private CodeStringView _mainCodeStringView;
        private readonly CodeStringCoordinator _codeStringCoordinator = new CodeStringCoordinator();
        public const float CodeStringMargin = 4f;
        private NSLayoutConstraint _mainCodeStringViewBottomConstraint;
        private List<UILabel> _codeHourLabels;
        private UILabel _lpricharLabel;

        public CodeStringsScene()
        {
            AddViews();
            ConstrainLayout();
        }

        private void ConstrainLayout()
        {
            var textWidth = _mainCodeStringView.GetTextWidth();
            var textWidthPlusPadding = textWidth * 3;
            var textHeight = _mainCodeStringView.GetTextHeight();

            ConstrainMainCodeString(textWidthPlusPadding, textHeight);
            ConstrainCodeHourLabels();
            ConstrainLpricharLabel();
        }

        private void ConstrainMainCodeString(nfloat textWidthPlusPadding, nfloat textHeight)
        {
            this.ConstrainLayout(() =>
                _mainCodeStringView.Frame.GetCenterX() == Frame.GetCenterX()
                && _mainCodeStringView.Frame.Width == textWidthPlusPadding
                && _mainCodeStringView.Frame.Height == textHeight
                && _mainCodeStringView.Frame.Bottom == Frame.Top
            );

            _mainCodeStringViewBottomConstraint = Constraints.First(i => i.FirstItem == _mainCodeStringView &&
                                                                         i.FirstAttribute == NSLayoutAttribute.Bottom);
        }

        private void ConstrainLpricharLabel()
        {
            var lastLetter = _codeHourLabels[_codeHourLabels.Count - 1];
            var firstLetter = _codeHourLabels[0];
            this.ConstrainLayout(() =>
                _lpricharLabel.Frame.Right == lastLetter.Frame.Right
                && _lpricharLabel.Frame.Left == firstLetter.Frame.Left
                && _lpricharLabel.Frame.Bottom == Frame.GetCenterY() - 15
                && _lpricharLabel.Frame.Height == 100
            );
        }

        private void ConstrainCodeHourLabels()
        {
            int charColumn = -7;
            foreach (var codeHourLabel in _codeHourLabels)
            {
                var codeStringView = GetCodeStringViewAtColumn(charColumn);
                this.ConstrainLayout(() =>
                    codeHourLabel.Frame.Top == Frame.GetCenterY() + 15
                    && codeHourLabel.Frame.GetCenterX() == codeStringView.Frame.GetCenterX()
                );
                charColumn++;
            }
        }

        private CodeStringView GetCodeStringViewAtColumn(int charColumn)
        {
            if (charColumn == 0) return _mainCodeStringView;
            return _codeStringCoordinator.GetCodeStringViewAtColumn(charColumn);
        }

        private void AddViews()
        {
            _mainCodeStringView = CodeStringCoordinator.AddCodeStringView(this);
            AddRemainingCodeStrings();
            _codeHourLabels = AddCodeHourLabels(this, CodeStringView.FontSize + 4);
            AddLpricharLabel();
        }

        private void AddLpricharLabel()
        {
            _lpricharLabel = this.AddLabel("lprichar", 85, UIColor.FromRGB(142, 164, 128));
            _lpricharLabel.Hidden = true;
            _lpricharLabel.TextAlignment = UITextAlignment.Right;
            _lpricharLabel.Alpha = 0f;
        }

        public async Task ShowLpricharShowText()
        {
            _lpricharLabel.Hidden = false;
            _lpricharLabel.Alpha = 0;
            await AnimateAsync(.5f, () =>
            {
                _lpricharLabel.Alpha = 1;
            });
        }

        private static List<UILabel> AddCodeHourLabels(UIView rootView, int fontSize)
        {
            var codeHourString = "code hour";
            var codeHourChars = codeHourString.ToCharArray();
            var labels = codeHourChars
                .Select(ch => rootView.AddLabel(ch.ToString(), fontSize, UIColor.FromRGB(121, 226, 131)))
                .ToList();
            return labels;
        }

        private void AddRemainingCodeStrings()
        {
            LayoutIfNeeded();
            var textWidth = _mainCodeStringView.GetTextWidth();
            var screenWidth = UIScreen.MainScreen.Bounds.Width;
            var halfScreenWidth = screenWidth / 2;
            var linesPerHalfScreen = halfScreenWidth / (textWidth + CodeStringMargin);
            var previousLeftCodeStringRow = _mainCodeStringView;
            var previousRightCodeStringRow = _mainCodeStringView;
            _codeStringCoordinator.TextWidth = textWidth;
            _codeStringCoordinator.TextHeight = _mainCodeStringView.GetTextHeight();
            _codeStringCoordinator.CenterCodeString = _mainCodeStringView;
            for (int column = 0; column < linesPerHalfScreen; column++)
            {
                var oneBasedColumn = column + 1;
                previousRightCodeStringRow = _codeStringCoordinator.AddToRightOf(oneBasedColumn, previousRightCodeStringRow, this);
                previousLeftCodeStringRow = _codeStringCoordinator.AddToLeftOf(-oneBasedColumn, previousLeftCodeStringRow, this);
            }
        }

        private const float CodeFallingDurationduration = 12f;

        public int AnimateOnce()
        {
            _mainCodeStringView.StartCharacterAnimations();
            _codeStringCoordinator.StartCharacterAnimations(delayInMs: 1000).FireAndForget();
            Zoom(delay: 1f);
            ShowLpricharShowText(delay: 5f);
            AnimateCodeFalling();
            return (int)CodeFallingDurationduration * 1000;
        }

        private void ShowLpricharShowText(float delay)
        {
            Animate(2f, delay, UIViewAnimationOptions.CurveLinear, () =>
            {
                foreach (var codeHourLabel in _codeHourLabels)
                {
                    codeHourLabel.Alpha = 1;
                }
            }, null);
        }

        private void AnimateCodeFalling()
        {
            _mainCodeStringViewBottomConstraint.Constant = UIScreen.MainScreen.Bounds.Height + _mainCodeStringView.GetTextHeight() + MaxDistanceFromCenter;
            AnimateNotify(CodeFallingDurationduration, 0, UIViewAnimationOptions.CurveLinear, LayoutIfNeeded, null);
        }

        private void Zoom(float delay)
        {
            var zoomTransform = CGAffineTransform.MakeScale(1.3f, 1.3f);
            AnimateNotify(7f, delay, UIViewAnimationOptions.CurveEaseIn, () => { Transform = zoomTransform; }, null);
        }

        public void Reset()
        {
            _mainCodeStringView.StopAnimation();
            _mainCodeStringViewBottomConstraint.Constant = 0;
            _codeStringCoordinator.StopAnimations();
            Transform = CGAffineTransform.MakeScale(1, 1);
            foreach (var codeHourLabel in _codeHourLabels)
            {
                codeHourLabel.Alpha = 0;
            }
            _lpricharLabel.Alpha = 0;
            _lpricharLabel.Hidden = true;
        }
    }
}