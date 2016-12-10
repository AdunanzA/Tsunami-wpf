using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;
using NLog;
using Humanizer;

namespace Tsunami.Models
{
    public class Preferences : INotifyPropertyChanged
    {
        private Logger log = LogManager.GetLogger("Preferences");

        public event PropertyChangedEventHandler PropertyChanged;

        private string _pathDownload;
        private bool _startWebOnAppLoad;
        private string _webAddress;
        private string _webPort;
        private bool _webUseAuth;
        private string _webUser;
        private string _webPassword;
        private long _streamingBufferSize;
        private bool _isDarkTheme;
        private string _themeColor;
        private string _accentColor;
        private string _logLevel;

        public string PathDownload { get { return _pathDownload; } set { if (_pathDownload != value) { _pathDownload = value; CallPropertyChanged("PathDownload"); } } }

        public bool StartWebOnAppLoad { get { return _startWebOnAppLoad; } set { if (_startWebOnAppLoad != value) { _startWebOnAppLoad = value; CallPropertyChanged("StartWebOnAppLoad"); } } }

        public string WebAddress { get { return _webAddress; } set { if (_webAddress != value) { _webAddress = value; CallPropertyChanged("WebAddress"); } } }

        public string WebPort { get { return _webPort; } set { if (_webPort != value) { _webPort = value; CallPropertyChanged("WebPort"); } } }

        public bool WebUseAuth { get { return _webUseAuth; } set { if (_webUseAuth != value) { _webUseAuth = value; CallPropertyChanged("WebUseAuth"); } } }

        public string WebUser { get { return _webUser; } set { if (_webUser != value) { _webUser = value; CallPropertyChanged("WebUser"); } } }

        public string WebPassword { get { return _webPassword; } set { if (_webPassword != value) { _webPassword = value; CallPropertyChanged("WebPassword"); } } }

        public long StreamingBufferSize { get { return _streamingBufferSize; } set { if (_streamingBufferSize != value) { _streamingBufferSize = value; CallPropertyChanged("streamingBufferSize"); } } }

        public NLog.LogLevel LogLevel
        {
            get
            {
                switch (_logLevel)
                {
                    case "fatal":
                        return NLog.LogLevel.Fatal;
                    case "error":
                        return NLog.LogLevel.Error;
                    case "warn":
                        return NLog.LogLevel.Warn;
                    case "info":
                        return NLog.LogLevel.Info;
                    case "debug":
                        return NLog.LogLevel.Debug;
                    case "trace":
                        return NLog.LogLevel.Trace;
                    default:
                        return NLog.LogLevel.Info;
                }
            }
            set
            {
                _logLevel = value.ToString().ToLowerInvariant();
                foreach (var rule in LogManager.Configuration.LoggingRules)
                {
                    rule.EnableLoggingForLevels(value, NLog.LogLevel.Fatal);
                }
                LogManager.ReconfigExistingLoggers();
                CallPropertyChanged("LogLevel");
            }
        }

        public List<ColorItem> ColorItems { get; }
        public List<ColorItem> AccentItems { get; }

        public bool IsDarkTheme {
            get { return _isDarkTheme; }
            set {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    new PaletteHelper().SetLightDark(_isDarkTheme);
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
                    Swatch sw = ColorItems.First(e => e.Name == _themeColor).Swatch;
                    new PaletteHelper().ReplacePrimaryColor(sw);
                    CallPropertyChanged("ThemeColor");
                }
            }
        }

        public string AccentColor
        {
            get { return _accentColor; }
            set
            {
                if (_accentColor != value)
                {
                    _accentColor = value;
                    Swatch sw = AccentItems.First(e => e.Name == _accentColor).Swatch;
                    new PaletteHelper().ReplaceAccentColor(sw);
                    CallPropertyChanged("AccentColor");
                }
            }
        }

        public Preferences()
        {
            ColorItems = new List<ColorItem>();
            AccentItems = new List<ColorItem>();

            foreach (Swatch item in new SwatchesProvider().Swatches)
            {
                System.Windows.Media.SolidColorBrush res = new System.Windows.Media.SolidColorBrush(item.ExemplarHue.Color);
                ColorItem ci = new ColorItem(res, item, item.Name.Humanize());
                ColorItems.Add(ci);

                if (item.IsAccented)
                {
                    System.Windows.Media.SolidColorBrush resA = new System.Windows.Media.SolidColorBrush(item.AccentExemplarHue.Color);
                    ColorItem cia = new ColorItem(resA, item, item.Name.Humanize());
                    AccentItems.Add(cia);
                }
            }
            reloadPreferenceFromFile();
        }

        public void reloadPreferenceFromFile()
        {
            Settings.User.readFromFile();

            PathDownload = Settings.User.PathDownload;
            StartWebOnAppLoad = Settings.User.StartWebOnAppLoad;
            StreamingBufferSize = Settings.User.StreamingBufferSize;
            WebAddress = Settings.User.WebAddress;
            WebPassword = Settings.User.WebPassword;
            WebPort = Settings.User.WebPort;
            WebUseAuth = Settings.User.WebUseAuth;
            WebUser = Settings.User.WebUser;
            StreamingBufferSize = Settings.User.StreamingBufferSize;
            IsDarkTheme = Settings.User.IsDarkTheme;
            ThemeColor = Settings.User.ThemeColor;
            AccentColor = Settings.User.AccentColor;
            LogLevel = NLog.LogLevel.FromString(Settings.User.LogLevel);

            log.Debug("loaded");
        }

        public void savePreferenceToFile()
        {
            Settings.User.PathDownload = PathDownload;
            Settings.User.StartWebOnAppLoad = StartWebOnAppLoad;
            Settings.User.StreamingBufferSize = StreamingBufferSize;
            Settings.User.WebAddress = WebAddress;
            Settings.User.WebPassword = WebPassword;
            Settings.User.WebPort = WebPort;
            Settings.User.WebUseAuth = WebUseAuth;
            Settings.User.WebUser = WebUser;
            Settings.User.StreamingBufferSize = StreamingBufferSize;
            Settings.User.IsDarkTheme = IsDarkTheme;
            Settings.User.ThemeColor = ThemeColor;
            Settings.User.AccentColor = AccentColor;
            Settings.User.LogLevel = LogLevel.ToString();

            Settings.User.writeToFile();
            log.Debug("saved");
        }

        private void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public class ColorItem
        {
            public System.Windows.Media.SolidColorBrush Color { get; set; }
            public Swatch Swatch { get; set; }
            public string Name { get; set; }

            public ColorItem(System.Windows.Media.SolidColorBrush color, Swatch swatch, string name)
            {
                Color = color;
                Swatch = swatch;
                Name = name;
            }
        }
    }
}
