using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using System.Windows;

namespace Tsunami.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public static class User
    {
        private static string _fileName = @"user.config";

        private static string _pathDownload = @"c:\download";
        private static bool _startWebOnAppLoad = true;
        private static string _webAddress = "localhost";
        private static string _webPort = "4242";
        private static bool _webUseAuth = false;
        private static string _webUser = "admin";
        private static string _webPassword = "admin";
        private static long _streamingBufferSize = 1048576; //1MB default size
        private static bool _isDarkTheme = true;
        private static string _themeColor = "Orange";
        private static string _accentColor = "Red";

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

        public static long StreamingBufferSize
        {
            get
            {
                return _streamingBufferSize;
            }
            set
            {
                _streamingBufferSize = value;
            }
        }

        public static bool IsDarkTheme
        {
            get
            {
                return _isDarkTheme;
            }

            set
            {
                _isDarkTheme = value;
            }
        }

        public static string ThemeColor
        {
            get
            {
                return _themeColor;
            }

            set
            {
                _themeColor = value;
            }
        }

        public static string AccentColor
        {
            get
            {
                return _accentColor;
            }

            set
            {
                _accentColor = value;
            }
        }

        public static void readFromFile()
        {
            if (File.Exists(_fileName))
            {
                XDocument doc = XDocument.Load(_fileName);
                foreach (XElement element in doc.Root.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "PathDownload":
                            PathDownload = element.Value;
                            break;
                        case "StartWebOnAppLoad":
                            StartWebOnAppLoad = bool.Parse(element.Value);
                            break;
                        case "WebAddress":
                            WebAddress = element.Value;
                            break;
                        case "WebPort":
                            WebPort = element.Value;
                            break;
                        case "WebUseAuth":
                            WebUseAuth = bool.Parse(element.Value);
                            break;
                        case "WebUser":
                            WebUser = element.Value;
                            break;
                        case "WebPassword":
                            WebPassword = element.Value;
                            break;
                        case "StreamingBufferSize":
                            _streamingBufferSize = long.Parse(element.Value);
                            break;
                        case "IsDarkTheme":
                            _isDarkTheme = bool.Parse(element.Value);
                            break;
                        case "ThemeColor":
                            _themeColor = element.Value;
                            break;
                        case "AccentColor":
                            _accentColor = element.Value;
                            break;
                        default:
                            break;
                    }
                }
            } else
            {
                writeToFile();
            }
        }

        public static void writeToFile()
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement("root"));
            doc.Root.Add(new XElement("PathDownload", PathDownload));
            doc.Root.Add(new XElement("StartWebOnAppLoad", StartWebOnAppLoad));
            doc.Root.Add(new XElement("WebAddress", WebAddress));
            doc.Root.Add(new XElement("WebPort", WebPort));
            doc.Root.Add(new XElement("WebUseAuth", WebUseAuth));
            doc.Root.Add(new XElement("WebUser", WebUser));
            doc.Root.Add(new XElement("WebPassword", WebPassword));
            doc.Root.Add(new XElement("StreamingBufferSize", StreamingBufferSize));
            doc.Root.Add(new XElement("IsDarkTheme", IsDarkTheme));
            doc.Root.Add(new XElement("ThemeColor", ThemeColor));
            doc.Root.Add(new XElement("AccentColor", AccentColor));
            doc.Save(_fileName);
        }
    }
}
