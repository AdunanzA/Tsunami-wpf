using System;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Meta.Vlc.Wpf;
using System.Windows.Controls.Primitives;
using System.IO;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per Player.xaml
    /// </summary>
    public partial class Player : Page
    {
        DispatcherTimer timer;

        bool isFullScreen = false;
        bool isDragging = false;
        VlcPlayer myPlayer = new VlcPlayer();
        public Player()
        {
            var vlcPath = Utils.GetWinVlcPath();

            if (Utils.IsWindowsOs())
            {
                Directory.SetCurrentDirectory(vlcPath);
            }

            InitializeComponent();

            //Player Settings
            myPlayer.Background = Brushes.Black;
            myPlayer.MouseDoubleClick += setFullScreen;
            myPlayer.CreateMode = PlayerCreateMode.NewVlcInstance;
            myPlayer.MouseMove += showProgressBar;           
            myPlayer.SetValue(Canvas.ZIndexProperty, -1);
            myGrid.Children.Add(myPlayer);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            volumeControl.Value = myPlayer.Volume;
            Stop.IsEnabled = false;
            Pause.IsEnabled = false;
        }

        private void showProgressBar(object sender, MouseEventArgs e)
        {
            if (isFullScreen)
            {
                playerStatus.Visibility = Visibility.Visible;
            }
        }
        
        private void hideProgressBar(object sender, MouseEventArgs e)
        {
            if (isFullScreen)
            {
                playerStatus.Visibility = Visibility.Collapsed;
            }
        }

        private void setFullScreen(object sender, MouseButtonEventArgs e)
        {
            if (!isFullScreen)
            {
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                playerStatus.Visibility = Visibility.Collapsed;
                isFullScreen = true;
            }
            else
            {
                Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                playerStatus.Visibility = Visibility.Visible;

                isFullScreen = false;
            }
        }
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            myPlayer.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
            
            myPlayer.Play();
            
            myPlayer.Stretch = Stretch.Fill;
            myPlayer.StretchDirection = StretchDirection.Both;
            Play.IsEnabled = false;
            Pause.IsEnabled = true;
            Stop.IsEnabled = true;
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            myPlayer.PauseOrResume();           
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            myPlayer.Stop();
            Stop.IsEnabled = false;
            Pause.IsEnabled = false;
            Play.IsEnabled = true;
        }

        private void manageVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            myPlayer.Volume = (int)volumeControl.Value;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging && myPlayer.VideoSource != null)
            {
                movieProgress.Minimum = 0;
                movieProgress.Maximum = myPlayer.Length.TotalSeconds;
                movieProgress.Value = myPlayer.Time.TotalSeconds;
            }
        }

        private void movieProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void movieProgrss_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            myPlayer.Time = TimeSpan.FromSeconds(movieProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(movieProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            volumeControl.Value = myPlayer.Volume += (e.Delta > 0) ? 1 : -1;
        }

    }
}
