using System.Linq;
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

        public CodeStringsScene()
        {
            AddViews();
            ConstrainLayout();
        }

        private void ConstrainLayout()
        {
            var textWidth = _mainCodeStringView.GetTextWidth() * 3;
            var textHeight = _mainCodeStringView.GetTextHeight();

            this.ConstrainLayout(() =>
                _mainCodeStringView.Frame.GetCenterX() == Frame.GetCenterX()
                && _mainCodeStringView.Frame.Width == textWidth
                && _mainCodeStringView.Frame.Height == textHeight
                && _mainCodeStringView.Frame.Bottom == Frame.Top
            );

            _mainCodeStringViewBottomConstraint = Constraints.First(i => i.FirstItem == _mainCodeStringView &&
                                                                         i.FirstAttribute == NSLayoutAttribute.Bottom);
        }

        private void AddViews()
        {
            _mainCodeStringView = CodeStringCoordinator.AddCodeStringView(this);
            AddRemainingCodeStrings();
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
            AnimateCodeFalling();
            return (int)CodeFallingDurationduration * 1000;
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
        }
    }
}