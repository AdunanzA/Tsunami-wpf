using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace Tsunami.Gui.Wpf
{
    public class Preferences : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _showAdvancedInterface;
        private static string _pathDownload;
        private static bool _startWebOnAppLoad;
        private static string _webAddress;
        private static string _webPort;
        private static bool _webUseAuth;
        private static string _webUser;
        private static string _webPassword;
        private static long _streamingBufferSize;
        private static bool _isDarkTheme;
        private static string _themeColor;

        public bool ShowAdvancedInterface { get { return _showAdvancedInterface; } set { if (_showAdvancedInterface != value) { _showAdvancedInterface = value; CallPropertyChanged("ShowAdvancedInterface"); } } }

        public string PathDownload { get { return _pathDownload; } set { if (_pathDownload != value) { _pathDownload = value; CallPropertyChanged("PathDownload"); } } }

        public bool StartWebOnAppLoad { get { return _startWebOnAppLoad; } set { if (_startWebOnAppLoad != value) { _startWebOnAppLoad = value; CallPropertyChanged("StartWebOnAppLoad"); } } }

        public string WebAddress { get { return _webAddress; } set { if (_webAddress != value) { _webAddress = value; CallPropertyChanged("WebAddress"); } } }

        public string WebPort { get { return _webPort; } set { if (_webPort != value) { _webPort = value; CallPropertyChanged("WebPort"); } } }

        public bool WebUseAuth { get { return _webUseAuth; } set { if (_webUseAuth != value) { _webUseAuth = value; CallPropertyChanged("WebUseAuth"); } } }

        public string WebUser { get { return _webUser; } set { if (_webUser != value) { _webUser = value; CallPropertyChanged("WebUser"); } } }

        public string WebPassword { get { return _webPassword; } set { if (_webPassword != value) { _webPassword = value; CallPropertyChanged("WebPassword"); } } }

        public long StreamingBufferSize { get { return _streamingBufferSize; } set { if (_streamingBufferSize != value) { _streamingBufferSize = value; CallPropertyChanged("streamingBufferSize"); } } }

        public bool IsDarkTheme {
            get { return _isDarkTheme; }
            set {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    string stheme = "BaseLight";
                    if (_isDarkTheme)
                    {
                        stheme = "BaseDark";
                    }
                    var theme = MahApps.Metro.ThemeManager.DetectAppStyle(Application.Current);
                    MahApps.Metro.ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, MahApps.Metro.ThemeManager.GetAppTheme(stheme));
                    CallPropertyChanged("IsDarkTheme");
                }
            }
        }

        public string ThemeColor
        {
            get { return _themeColor; }
            set
            {
                if (_themeColor != value)
                {
                    _themeColor = value;
                    var theme = MahApps.Metro.ThemeManager.DetectAppStyle(Application.Current);
                    MahApps.Metro.ThemeManager.ChangeAppStyle(Application.Current, MahApps.Metro.ThemeManager.GetAccent(value), theme.Item1);
                    CallPropertyChanged("ThemeColor");
                }
            }
        }

        public Preferences()
        {
            _showAdvancedInterface = false;
            reloadPreferenceFromFile();
        }

        public void reloadPreferenceFromFile()
        {
            Settings.User.readFromFile();
            PathDownload = Settings.User.PathDownload;
            StartWebOnAppLoad = Settings.User.StartWebOnAppLoad;
            StreamingBufferSize = Settings.User.streamingBufferSize;
            WebAddress = Settings.User.WebAddress;
            WebPassword = Settings.User.WebPassword;
            WebPort = Settings.User.WebPort;
            WebUseAuth = Settings.User.WebUseAuth;
            WebUser = Settings.User.WebUser;
            StreamingBufferSize = Settings.User.streamingBufferSize;
            IsDarkTheme = Settings.User.IsDarkTheme;
            ThemeColor = Settings.User.ThemeColor;
        }

        public void savePreferenceToFile()
        {
            Settings.User.PathDownload = PathDownload;
            Settings.User.StartWebOnAppLoad = StartWebOnAppLoad;
            Settings.User.streamingBufferSize = StreamingBufferSize;
            Settings.User.WebAddress = WebAddress;
            Settings.User.WebPassword = WebPassword;
            Settings.User.WebPort = WebPort;
            Settings.User.WebUseAuth = WebUseAuth;
            Settings.User.WebUser = WebUser;
            Settings.User.streamingBufferSize = StreamingBufferSize;
            Settings.User.IsDarkTheme = IsDarkTheme;
            Settings.User.ThemeColor = ThemeColor;
            Settings.User.writeToFile();
        }

        private void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
