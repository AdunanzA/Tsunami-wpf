using System;
using System.Threading.Tasks;
using Meta.Vlc.Wpf;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;



namespace Tsunami.Gui.Wpf
{
    public class PlayerViewModel
    {
        static public VlcPlayer vlcPlayer = null;
        DispatcherTimer timer = null;
        static private PlayerViewModel ptr = null;
        static private Image DisplayImage = null;
        


        public ICommand _playClick { get; set; }
        public ICommand _stopClick { get; set; }
        public ICommand _pauseClick { get; set; }


        private Player _player { get; set; }

        public PlayerViewModel()
        {
            ptr = this;
            this._playClick = new CommandExecutor(PlayClick, CanExecute);
            this._stopClick = new CommandExecutor(StopClick, CanExecute);
            this._pauseClick = new CommandExecutor(PauseClick, CanExecute);

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
            if (vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Playing) return;
            if (vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Paused)
            {
                PauseClick(parameter);
                return;
            }
            vlcPlayer.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));

            Task.Run(() =>
            {
                vlcPlayer.Play();
            });

            vlcPlayer.LengthChanged += new EventHandler(OnMediaLengthChanged);
            timer.Start();

            player.FullScreenEnabled = true;
            player.PlayEnabled = true;
            player.PauseEnabled = true;
            player.StopEnabled = true;
        }


        private void OnMediaLengthChanged(object sender, EventArgs e)
        {
            player.MaxMovieTime = vlcPlayer.Length.TotalSeconds;
        }

        public void StopClick(object parameter)
        {
            timer.Stop();
            vlcPlayer.LengthChanged -= OnMediaLengthChanged;


            Task.Run(() =>
            {
                vlcPlayer.Stop();
                vlcPlayer.RebuildPlayer();
            });

            player.StopEnabled = false;
            player.PauseEnabled = false;
            player.PlayEnabled = true;
        }



        public void PauseClick(object parameter)
        {
            vlcPlayer.PauseOrResume();
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
            i.Source = vlcPlayer.VideoSource;
            i.Stretch = Stretch.Fill;
            vlcPlayer.VideoSourceChanged += PlayerOnVideoSourceChanged;
            vlcPlayer.Background = Brushes.Black;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);

            player.VolumeValue = vlcPlayer.Volume;
            player.StopEnabled = false;
            player.PauseEnabled = false;
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

        private void timer_Tick(object sender, EventArgs e)
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

        static public void HandleMouseWheel(MouseWheelEventArgs e)
        {
            if (ptr != null)
                ptr.player.VolumeValue = vlcPlayer.Volume += (e.Delta > 0) ? 1 : -1;
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

    }
}
