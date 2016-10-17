using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace Tsunami
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static UserControl _addPage;
        public static UserControl _listPage;
        public static UserControl _playerPage;
        public static UserControl _settingsPage;

        public MainWindow()
        {
            InitializeComponent();
            SetLanguageDictionary();

            var verMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            var verMin = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            var verRev = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
            var title = Title + " " + +verMajor + "." + verMin + verRev;
            Title = title;

            Closing += Window_Closing;

            _addPage = new Pages.Add();
            _listPage = new Pages.List();
            _playerPage = new Pages.Player();
            _settingsPage = new SettingsPage();

            Classes.Switcher.pageSwitcher = this;
            Classes.Switcher.Switch(_addPage);

        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri(Environment.CurrentDirectory + "/Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "it-IT":
                    dict.Source = new Uri(Environment.CurrentDirectory + "/Resources/italian.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    dict.Source = new Uri(Environment.CurrentDirectory + "/Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();

            // Save MainWindow Settings
            Properties.Settings.Default.Save();

            // Save user preferences
            Settings.User.writeToFile();

            // dispose fullscreen xaml
            //fullScreenWindow.Dispose();

            // terminate streamingmanager
            Streaming.StreamingManager.Terminate?.Invoke(this, null);

            _addPage = null;
            _listPage = null;
            _playerPage = null;
            _settingsPage = null;

            Environment.Exit(0);
        }

        public void Navigate(UserControl nextPage)
        {
            mainContent.Content = nextPage;
        }

        private void AddPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainContent.Content = _addPage;
        }

        private void ListPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainContent.Content = _listPage;
        }

        private void PlayerPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainContent.Content = _playerPage;
        }

        private void SettingsPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mainContent.Content = _settingsPage;
        }
    }
}
