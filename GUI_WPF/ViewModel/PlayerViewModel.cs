using System;
using System.Threading.Tasks;
using Meta.Vlc.Wpf;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;


namespace Tsunami.Gui.Wpf
{
    public class PlayerViewModel
    {
        static public VlcPlayer vlcPlayer = null;
        DispatcherTimer timer = null;
        DispatcherTimer hideBarTimer = null;
        bool isFullScreen = false;
        static PlayerViewModel ptr = null;
        static Image DisplayImage = null;


        public ICommand _playClick { get; set; }
        public ICommand _stopClick { get; set; }
        public ICommand _pauseClick { get; set; }
        public ICommand _fullscreenClick { get; set; }


        private Player _player { get; set; }

        public PlayerViewModel()
        {
            ptr = this;
            this._playClick = new CommandExecutor(PlayClick, CanExecute);
            this._stopClick = new CommandExecutor(StopClick, CanExecute);
            this._pauseClick = new CommandExecutor(PauseClick, CanExecute);
            this._fullscreenClick = new CommandExecutor(FullScreenClick, CanExecute);

            _player = new Player();
            _player.VolumeChanged += new ChangedEventHandler(UpdateVolumeChange);
            _player.MovieProgressChanged += new ChangedEventHandler(UpdateMovieProgress);

        }

        public Player player
        {
            get
            {
                return _player;
            }
        }

        static public void ImageLoadCompleted(ref Image i)
        {
            if (ptr != null)
            {
                DisplayImage = i;
                ptr.InitializeVLC(ref DisplayImage);
            }
        }


        public void PlayClick(object parameter)
        {
            vlcPlayer.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));

            Task.Run(() =>
            {
                vlcPlayer.Play();
            });

            vlcPlayer.LengthChanged += new EventHandler(OnMediaLengthChanged);
            timer.Start();
            hideBarTimer.Start();

            //FullScreen.IsEnabled = true;
            //Play.IsEnabled = false;
            // Pause.IsEnabled = true;
            //Stop.IsEnabled = true;
        }


        public void OnMediaLengthChanged(object sender, EventArgs e)
        {
            player.MaxMovieTime = vlcPlayer.Length.TotalSeconds;
        }

        public void StopClick(object parameter)
        {
            timer.Stop();
            hideBarTimer.Stop();
            vlcPlayer.LengthChanged -= OnMediaLengthChanged;


            Task.Run(() =>
            {
                vlcPlayer.Stop();
                vlcPlayer.RebuildPlayer();
            });

            /*if (isFullScreen)
            {
                fscreenGrid.Children.Clear();
                myGrid.Children.Add(DisplayImage);
                myGrid.Children.Add(playerStatus);
                fscreen.Close();
                fscreenGrid = null;
                isFullScreen = false;
                this.Show();
            }

            FullScreen.IsEnabled = false;
            Stop.IsEnabled = false;
            Pause.IsEnabled = false;
            Play.IsEnabled = true;*/
        }



        public void PauseClick(object parameter)
        {
            vlcPlayer.PauseOrResume();
        }

        public void FullScreenClick(object parameter)
        {
            MessageBox.Show("Executing command 3");
        }

        void InitializeVLC(ref Image i)
        {
            //Player Settings
            string startupPath = System.IO.Directory.GetCurrentDirectory();

            var vlcPath = Utils.GetWinVlcPath();

            if (Utils.IsWindowsOs())
            {
                Directory.SetCurrentDirectory(vlcPath);
            }

            vlcPlayer = new VlcPlayer(i.Dispatcher);
            vlcPlayer.Initialize(vlcPath, new string[] { "-I", "dummy", "--ignore-config", "--no-video-title" });
            vlcPlayer.VideoSourceChanged += PlayerOnVideoSourceChanged;
            vlcPlayer.Background = Brushes.Black;

            DisplayImage.MouseMove += showProgressBar;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);

            hideBarTimer = new DispatcherTimer();
            hideBarTimer.Interval = TimeSpan.FromSeconds(5);
            hideBarTimer.Tick += new EventHandler(HideBar_Tick);

            player.VolumeValue = vlcPlayer.Volume;
            //Stop.IsEnabled = false;
            //Pause.IsEnabled = false;
            //End Player Settings

            //Restore default path after vlc initialization
            Directory.SetCurrentDirectory(startupPath);
        }

        public void PlayerOnVideoSourceChanged(object sender, VideoSourceChangedEventArgs videoSourceChangedEventArgs)
        {

            DisplayImage.Dispatcher.BeginInvoke(new Action(() =>
            {
                DisplayImage.Source = videoSourceChangedEventArgs.NewVideoSource;
            }));
        }

        public void showProgressBar(object sender, MouseEventArgs e)
        {
            if (isFullScreen)
            {
                player.PlayerStatusVisibility = false;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if (vlcPlayer.VideoSource != null)
            {
                player.MovieProgress = vlcPlayer.Time.TotalSeconds;
                player.ProgressTime = TimeSpan.FromSeconds(player.MovieProgress).ToString(@"hh\:mm\:ss");
            }
        }

        public void UpdateVolumeChange()
        {
            vlcPlayer.Volume = player.VolumeValue;
        }

        public void UpdateMovieProgress()
        {
            vlcPlayer.Time = TimeSpan.FromSeconds(player.MovieProgress);
            player.ProgressTime = TimeSpan.FromSeconds(player.MovieProgress).ToString(@"hh\:mm\:ss");
        }

        public void HideBar_Tick(object sender, EventArgs e)
        {
            if (isFullScreen)
            {
                player.PlayerStatusVisibility = false;
                Mouse.OverrideCursor = Cursors.None;
            }
        }

        static public void HandleMouseWheel(MouseWheelEventArgs e)
        {
            if (ptr != null)
                ptr.player.VolumeValue = vlcPlayer.Volume += (e.Delta > 0) ? 1 : -1;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
