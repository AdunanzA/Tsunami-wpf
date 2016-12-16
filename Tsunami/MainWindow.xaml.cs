using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tsunami
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly UserControl _addPage = new Pages.Add();
        public static readonly UserControl _listPage = new Pages.List();
        public static readonly UserControl _playerPage = new Pages.Player();
        public static readonly UserControl _settingsPage = new Pages.SettingsPage();
        public static readonly UserControl _graphicsPage = new Pages.GraphicsPage();

        ViewModel.TsunamiViewModel tvm;

        public MainWindow()
        {
            InitializeComponent();
            SetLanguageDictionary();

            tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");

            var verMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            var verMin = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            var verRev = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
            titleBlock.Text = Title + " " + verMajor + "." + verMin + verRev;
            
            Closing += Window_Closing;

            Classes.Switcher.PageSwitcher = this;

            if (ViewModel.TsunamiViewModel.TorrentList.Count > 0)
            {
                // we have torrent, switch to download list
                Classes.Switcher.Switch(_listPage);
            } else
            {
                Classes.Switcher.Switch(_addPage);
            }
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.ToString())
            {
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
            //Streaming.StreamingManager.Terminate?.Invoke(this, null);

            //_addPage = null;
            //_listPage = null;
            //_playerPage = null;
            //_settingsPage = null;

            tvm.Terminate();

            tsunamiNotifyIcon.Dispose();

            Environment.Exit(0);
        }

        public void Navigate(UserControl nextPage)
        {
            mainContent.Content = nextPage;
        }

        private void AddPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Classes.Switcher.Switch(_addPage);
        }

        private void ListPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //string title = "Tsunami";
            //string text = "You are seeing Tsunami list!";
            //tsunamiNotifyIcon.ShowBalloonTip(title, text, tsunamiNotifyIcon.Icon, true);
            Classes.Switcher.Switch(_listPage);
        }

        private void PlayerPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Classes.Switcher.Switch(_playerPage);
        }

        private void SettingsPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Classes.Switcher.Switch(_settingsPage);
        }

        private void GraphicsPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Classes.Switcher.Switch(_graphicsPage);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // vertical scroll with mouse wheel
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnResizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void OnMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ContentControl_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void OnPauseResumeLibTorrent_Click(object sender, RoutedEventArgs e)
        {
            tvm.PauseResumeSession();
        }

        private async void Logo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var aboutUs = new Pages.Dialogs.About();
            await MaterialDesignThemes.Wpf.DialogHost.Show(aboutUs, "RootDialog");
        }
    }
}
