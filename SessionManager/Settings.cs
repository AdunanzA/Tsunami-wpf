using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tsunami
{
    public static class Settings
    {
        /// <summary>Per ora metto sul desktop, 
        /// TODO: controllare se il settaggio è visto nelle altre classi
        /// </summary>
        public static string PATH_DOWNLOAD = @"c:\download";
        public static bool START_WEB_ON_APPLICATION_LOAD = false;
        public static string WEB_URL = "localhost";
        public static string WEB_PORT = "4242";
        public static bool WEB_USE_AUTH = false;
        public static string WEB_USER = "admin";
        public static string WEB_PW = "admin";
        public static int DISPATCHER_INTERVAL = 500; // how many times torrents must be notified between Tsunami application. In milliseconds. Default value 1000. Should not be lower than 500
    }
}
