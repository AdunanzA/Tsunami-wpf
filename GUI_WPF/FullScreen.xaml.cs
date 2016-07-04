using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per FullScreen.xaml
    /// </summary>
    public partial class FullScreen : Window
    {
        private bool isFullScreen = false;
        DispatcherTimer hideBarTimer = null;
        private MainWindow mainWindow = null;

        public FullScreen(MainWindow m)
        {
            InitializeComponent();
            mainWindow = m;

            InitializeFullScreen();
            mainWindow.DisplayImage.MouseMove += showProgressBar;

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
                mainWindow.normalGrid.Children.Clear();
                fullScreenGrid.Children.Add(mainWindow.imageBorder);
                fullScreenGrid.Children.Add(mainWindow.playerStatus);
                Show();
            }
            else
            {
                fullScreenGrid.Children.Clear();
                mainWindow.normalGrid.Children.Add(mainWindow.imageBorder);
                mainWindow.normalGrid.Children.Add(mainWindow.playerStatus);
                Hide();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            isFullScreen = !isFullScreen;
        }

        public void showProgressBar(object sender, MouseEventArgs e)
        {
            if (isFullScreen)
            {
                mainWindow.playerStatus.Visibility = Visibility.Visible;
                Mouse.OverrideCursor = Cursors.Arrow;
                hideBarTimer.Start();
            }
        }

        private void HideBar_Tick(object sender, EventArgs e)
        {
            if (isFullScreen)
            {
                mainWindow.playerStatus.Visibility = Visibility.Collapsed;
                Mouse.OverrideCursor = Cursors.None;
                hideBarTimer.Stop();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
