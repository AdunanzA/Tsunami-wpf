using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tsunami.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public static class User
    {
        private static string _pathDownload = @"c:\download";
        private static bool _startWebOnAppLoad = true;
        private static string _webAddress = "localhost";
        private static string _webPort = "4242";
        private static bool _webUseAuth = false;
        private static string _webUser = "admin";
        private static string _webPassword = "admin";

        public static string PathDownload
        {
            get
            {
                return _pathDownload;
            }

            set
            {
                _pathDownload = value;
            }
        }

        public static bool StartWebOnAppLoad
        {
            get
            {
                return _startWebOnAppLoad;
            }

            set
            {
                _startWebOnAppLoad = value;
            }
        }

        public static string WebAddress
        {
            get
            {
                return _webAddress;
            }

            set
            {
                _webAddress = value;
            }
        }

        public static string WebPort
        {
            get
            {
                return _webPort;
            }

            set
            {
                _webPort = value;
            }
        }

        public static bool WebUseAuth
        {
            get
            {
                return _webUseAuth;
            }

            set
            {
                _webUseAuth = value;
            }
        }

        public static string WebUser
        {
            get
            {
                return _webUser;
            }

            set
            {
                _webUser = value;
            }
        }

        public static string WebPassword
        {
            get
            {
                return _webPassword;
            }

            set
            {
                _webPassword = value;
            }
        }

        public static void readFromFile()
        {

        }

        public static void writeToFile()
        {

        }
    }
}
