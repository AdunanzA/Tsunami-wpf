using System;
using System.Threading.Tasks;
using Meta.Vlc.Wpf;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tsunami.Streaming
{
    public class PlayerViewModel
    {
        static public VlcPlayer vlcPlayer = null;
        DispatcherTimer timer = null;
        static private Image DisplayImage = null;
        private object _lastMovie = new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi");

        public ICommand _playClick { get; set; }
        public ICommand _stopClick { get; set; }
        public ICommand _pauseClick { get; set; }

        private Player _player { get; set; }

        

        public PlayerViewModel()
        {
            this._playClick = new CommandExecutor(PlayClick, CanExecute);
            this._stopClick = new CommandExecutor(StopClick, CanExecute);
            this._pauseClick = new CommandExecutor(PauseClick, CanExecute);

            _player = new Player();
            _player.VolumeChanged += new ChangedEventHandler(UpdateVolumeChange);
            _player.MovieProgressChanged += new ChangedEventHandler(UpdateMovieProgress);

            StreamingManager.SetSurface += new EventHandler<Image>(LoadSurface);
            StreamingManager.Terminate += new EventHandler(Terminate);
            StreamingManager.PlayUri += new EventHandler<Uri>(PlayUri);
            StreamingManager.PlayMediaPath += new EventHandler<string>(PlayMediaPath);
            StreamingManager.SetPauseButtonStatus += new EventHandler<bool>(SetPauseButtonStatus);
            StreamingManager.SetPauseButtonStatus += new EventHandler<bool>(SetPlayButtonStatus);
            StreamingManager.SetPauseButtonStatus += new EventHandler<bool>(SetStopButtonStatus);

        }

        private void SetStopButtonStatus(object sender, bool e)
        {
            player.StopEnabled = e;
        }

        private void SetPlayButtonStatus(object sender, bool e)
        {
            player.PlayEnabled = e;
        }

        private void SetPauseButtonStatus(object sender, bool e)
        {
            player.PauseEnabled = e;
        }

        public Player player
        {
            get
            {
                return _player;
            }
        }

        private void LoadSurface(object sender, Image i)
        {
            DisplayImage = i;
            DisplayImage.MouseWheel += HandleMouseWheel;
            InitializeVLC(ref DisplayImage);
        }

        public void PlayMediaPath(object sender, string mediaPath)
        {
            if (vlcPlayer == null) return;

            if(vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Playing ||
               vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Paused)
            {
                StopClick(null);
            }
            
            _lastMovie = mediaPath;
            PlayClick(_lastMovie);
        }

        private void PlayUri(object sender,Uri uri)
        {
            if (vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Playing ||
               vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Paused)
            {
                StopClick(null);
            }

            _lastMovie = uri;
            PlayClick(_lastMovie);
        }

        public void PlayClick(object parameter)
        {
            if (_lastMovie is Uri)
                vlcPlayer.LoadMedia((Uri)_lastMovie);
            else if (_lastMovie is string)
                vlcPlayer.LoadMedia((string)_lastMovie);
            else return;

            Task.Run(() =>
            {
                vlcPlayer.Play();
            });

            vlcPlayer.LengthChanged += new EventHandler(OnMediaLengthChanged);
            timer.Start();

            player.FullScreenEnabled = true;
            player.PlayEnabled = false;
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
                vlcPlayer.RebuildPlayer();
            });

            player.StopEnabled = false;
            player.PauseEnabled = false;
            player.PlayEnabled = true;
            if(SessionManager.IsStreaming())
            {
                SessionManager.StopStreaming();
            }
        }

        public void PauseClick(object parameter)
        {
            vlcPlayer.PauseOrResume();
        }

        private void InitializeVLC(ref Image i)
        {
            //Player Settings
            string startupPath = System.IO.Directory.GetCurrentDirectory();

            var vlcPath = Utils.GetWinVlcPath();
            
            if (Utils.IsWindowsOs())
            {
                Directory.SetCurrentDirectory(vlcPath);
            }

            vlcPlayer = new VlcPlayer(i.Dispatcher);
            vlcPlayer.Initialize(vlcPath, new string[] { "-I", "dummy", "--ignore-config", "--no-video-title"});
            i.Source = vlcPlayer.VideoSource;
            i.Stretch = Stretch.Fill;
            
            vlcPlayer.EndBehavior = EndBehavior.Nothing;
            vlcPlayer.VideoSourceChanged += PlayerOnVideoSourceChanged;
            vlcPlayer.VlcMediaPlayer.EndReached += OnEndReached;

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

        private void OnEndReached(object sender, Meta.Vlc.ObjectEventArgs<Meta.Vlc.Interop.Media.MediaState> e)
        {
            StopClick(sender);
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

        private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            player.VolumeValue = vlcPlayer.Volume += (e.Delta > 0) ? 1 : -1;
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void Terminate(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                vlcPlayer?.Dispose();
            });
        }
    }
}
