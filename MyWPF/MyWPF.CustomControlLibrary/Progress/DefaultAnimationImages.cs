using System.ComponentModel;
using System.Drawing;

namespace MyWPF.CustomControlLibrary
{
    [Description("Default Animation Images class.")]
    public class DefaultAnimationImages
    {
        private static Image[] _clockImages;
        private static Image[] _simpleImages;

        [Description("Constructor.")]
        private DefaultAnimationImages()
        {
        }

        [Description("The image array for the Clock animation.")]
        public static Image[] ClockAnimation
        {
            get
            {
                if (_clockImages != null) return _clockImages;
                _clockImages = new Image[11];
                for (int num1 = 0; num1 < _clockImages.Length; num1++)
                {
                    _clockImages[num1] = new Bitmap(typeof(DefaultAnimationImages), string.Format("Progress.ProgressThread.ClockAnimation.AnimationCell_{0:00}.png", num1 + 1));
                }
                return _clockImages;
            }
        }

        [Description("The image array for the simple animation.")]
        public static Image[] SimpleAnimation
        {
            get
            {
                if (_simpleImages != null) return _simpleImages;
                _simpleImages = new Image[4];
                for (int num1 = 0; num1 < _simpleImages.Length; num1++)
                {
                    _simpleImages[num1] = new Bitmap(typeof(DefaultAnimationImages), string.Format("Progress.ProgressThread.SimpleAnimation.AnimationCell_{0:00}.png", num1 + 1));
                }
                return _simpleImages;
            }
        }

    }
}