using System;
using System.Windows.Controls;

namespace Tsunami.Streaming
{
    public static class StreamingManager
    {
        public static EventHandler<Image> SetSurface;
        public static EventHandler Terminate;
        public static EventHandler<Uri> PlayUri;
        public static EventHandler<string> PlayMediaPath;
        public static EventHandler<bool> SetPlayButtonStatus;
        public static EventHandler<bool> SetPauseButtonStatus;
        public static EventHandler<bool> SetStopButtonStatus;
    }
}
