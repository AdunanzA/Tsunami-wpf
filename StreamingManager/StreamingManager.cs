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
    }
}
