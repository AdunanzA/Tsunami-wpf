using System.Windows.Controls;

namespace Tsunami
{
    public static class StreamingManager
    {
        private static PlayerViewModel _pwm = new PlayerViewModel();
        public static PlayerViewModel Streaming
        {
            get
            {
                return _pwm;
            }
        }

        public static void SetSurface(ref Image surface)
        {
            _pwm.LoadSurface(ref surface);
        }

        public static void Terminate()
        {
            _pwm.Terminate();
        }
    }
}
