using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;

namespace Tsunami.Gui.Wpf
{
    public partial class PlayerFullScreen :IDisposable
    {
        private Window fullScreenWindow = null;
        private Grid fullScreenGrid = null;
        private bool isFullScreen = false;
        DispatcherTimer hideBarTimer = null;
        private MainWindow mainWindow = null;

        public PlayerFullScreen(MainWindow m)
        {
            mainWindow = m;
            InitializeFullScreen();
            mainWindow.DisplayImage.MouseMove += showProgressBar;

            hideBarTimer = new DispatcherTimer();
            hideBarTimer.Interval = TimeSpan.FromSeconds(5);
            hideBarTimer.Tick += new EventHandler(HideBar_Tick);
        }

        private void InitializeFullScreen()
        {
            fullScreenWindow = new Window();
            fullScreenGrid = new Grid();

            fullScreenWindow.WindowState = WindowState.Maximized;
            fullScreenWindow.WindowStyle = WindowStyle.None;
            fullScreenWindow.Content = fullScreenGrid;
            fullScreenWindow.Topmost = true;
            fullScreenWindow.Hide();
        }

        public void SetFullScreen()
        {
            if (!isFullScreen)
            {
                mainWindow.normalGrid.Children.Clear();
                fullScreenGrid.Children.Add(mainWindow.DisplayImage);
                fullScreenGrid.Children.Add(mainWindow.playerStatus);
                fullScreenWindow.Show();
            }

            else
            {
                fullScreenGrid.Children.Clear();
                mainWindow.normalGrid.Children.Add(mainWindow.DisplayImage);
                mainWindow.normalGrid.Children.Add(mainWindow.playerStatus);
                fullScreenWindow.Hide();
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
            fullScreenWindow.Close();
            fullScreenGrid = null;
            fullScreenWindow = null;
        }
    }
}
