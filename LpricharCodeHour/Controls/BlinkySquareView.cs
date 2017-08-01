using System.Threading.Tasks;
using UIKit;

namespace LpricharCodeHour.Controls
{
    public sealed class BlinkySquareView : UIView
    {
        private bool _isBlinking = false;
        private readonly UIColor _backgroundColor = UIColor.LightGray;

        public BlinkySquareView()
        {
            BackgroundColor = _backgroundColor;
            EnsureBlinking();
        }

        private async void EnsureBlinking()
        {
            while (_isBlinking)
            {
                BackgroundColor = _backgroundColor;
                await Task.Delay(500);
                BackgroundColor = UIColor.Clear;
                await Task.Delay(500);
            }
        }

        public void Start()
        {
            _isBlinking = true;
            EnsureBlinking();
        }

        public void Stop()
        {
            _isBlinking = false;
            BackgroundColor = _backgroundColor;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _isBlinking = false;
        }
    }
}