using System;
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

        public static void PlayMedia(Uri uri)
        {
            _pwm.PlayMedia(uri);
        }

        public static void PlayMedia(string mediaPath)
        {
            _pwm.PlayMedia(mediaPath);
        }
    }
}
