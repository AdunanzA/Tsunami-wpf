using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tsunami
{
    /// <summary>
    /// Logica di interazione per FullScreen.xaml
    /// </summary>
    public partial class FullScreen : Window, IDisposable
    {
        private bool isFullScreen = false;
        DispatcherTimer hideBarTimer = null;
        private MainWindow mainWindow = null;

        public FullScreen(MainWindow m)
        {
            InitializeComponent();
            mainWindow = m;

            InitializeFullScreen();

            this.MouseMove += ShowProgressBar;

            hideBarTimer = new DispatcherTimer();
            hideBarTimer.Interval = TimeSpan.FromSeconds(5);
            hideBarTimer.Tick += new EventHandler(HideBar_Tick);
        }

        private void InitializeFullScreen()
        {
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            Topmost = true;
            Hide();
        }

        public void SetFullScreen()
        {
            if (!isFullScreen)
            {
                //mainWindow.playerGrid.Children.Clear();
                //this.fullScreenGrid.Children.Insert(0, mainWindow.playerStatus);
                //this.fullScreenGrid.Children.Insert(1, (UIElement)Tsunami.Streaming.PlayerViewModel.vlcPlayer);
                //Show();
                //mainWindow.playerStatus.Visibility = Visibility.Collapsed;
            }
            else
            {
                //fullScreenGrid.Children.Clear();
                //mainWindow.playerGrid.Children.Insert(0, mainWindow.playerStatus);
                //mainWindow.playerGrid.Children.Insert(1, (UIElement)Tsunami.Streaming.PlayerViewModel.vlcPlayer);
                //Hide();
                //Mouse.OverrideCursor = Cursors.Arrow;
            }
            isFullScreen = !isFullScreen;
        }

        public void ShowProgressBar(object sender, MouseEventArgs e)
        {
            if (isFullScreen)
            {
                //mainWindow.playerStatus.Visibility = Visibility.Visible;
                //Mouse.OverrideCursor = Cursors.Arrow;
                //hideBarTimer.Start();
            }
        }

        private void HideBar_Tick(object sender, EventArgs e)
        {
            if (isFullScreen)
            {               
                //mainWindow.playerStatus.Visibility = Visibility.Collapsed;
                //Mouse.OverrideCursor = Cursors.None;
                //hideBarTimer.Stop();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
