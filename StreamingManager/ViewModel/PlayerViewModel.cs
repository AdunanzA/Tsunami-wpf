using System;
using System.Threading.Tasks;
using Vlc.DotNet.Wpf;
using Vlc.DotNet.Core.Interops;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Vlc.DotNet.Forms;
using System.Windows.Data;
using System.Windows;

namespace Tsunami.Streaming
{
    public class PlayerViewModel
    {
        static public Vlc.DotNet.Wpf.VlcControl vlcPlayer = null;
        DispatcherTimer timer = null;
        static private Grid DisplayGrid = null;
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

            StreamingManager.SetSurface += new EventHandler<Grid>(LoadSurface);
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

        private void LoadSurface(object sender, Grid g)
        {
            DisplayGrid = g;
            DisplayGrid.MouseWheel += HandleMouseWheel;
            InitializeVLC(ref g);
        }

        public void PlayMediaPath(object sender, string mediaPath)
        {
            if (vlcPlayer == null) return;

            /*if(vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Playing   ||
               vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Paused)
            {
                StopClick(null);
            }*/
            
            _lastMovie = mediaPath;
            PlayClick(_lastMovie);
        }

        private void PlayUri(object sender, Uri uri)
        {
            /*if (vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Playing ||
               vlcPlayer.State == Meta.Vlc.Interop.Media.MediaState.Paused)
            {
                StopClick(null);
            }*/

            _lastMovie = uri;
            PlayClick(_lastMovie);
        }

        public void PlayClick(object parameter)
        {
            /*if(vlcPlayer.MediaPlayer.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused)
            {
                player.PlayEnabled = false;
                player.PauseEnabled = true;
                vlcPlayer.MediaPlayer.Play();
                return;
            }*/

            if (_lastMovie is Uri)
                vlcPlayer.MediaPlayer.Play((Uri)_lastMovie);
            else if (_lastMovie is string)
                vlcPlayer.MediaPlayer.Play(new FileInfo((string)_lastMovie));
            else return;


            vlcPlayer.MediaPlayer.TimeChanged += OnMediaLengthChanged;
            timer.Start();

            player.FullScreenEnabled = true;
            player.PlayEnabled = false;
            player.PauseEnabled = true;
            player.StopEnabled = true;
        }

        private void OnMediaLengthChanged(object sender,  EventArgs e)
        {
            player.MaxMovieTime = vlcPlayer.MediaPlayer.Length;
        }

        public void StopClick(object parameter)
        {
            timer.Stop();
            vlcPlayer.MediaPlayer.TimeChanged -= OnMediaLengthChanged;

            vlcPlayer.MediaPlayer.Stop();

            player.StopEnabled = false;
            player.PauseEnabled = false;
            player.PlayEnabled = true;
            player.MovieProgress = 0;
            player.ProgressTime = "00:00:00";

            if (SessionManager.Instance.IsStreaming())
            {
                SessionManager.Instance.StopStreaming();
            }
        }

        public void PauseClick(object parameter)
        {
            player.PlayEnabled = true;
            player.PauseEnabled = false;
            vlcPlayer.MediaPlayer.Pause();
        }

        private void InitializeVLC(ref Grid g)
        {
            //Player Settings

            string startupPath = System.IO.Directory.GetCurrentDirectory();

            var vlcPath = Utils.GetWinVlcPath();
            
            if (Utils.IsWindowsOs())
            {
                Directory.SetCurrentDirectory(vlcPath);
            }

            vlcPlayer = new Vlc.DotNet.Wpf.VlcControl();
            vlcPlayer.Background = Brushes.Black;
            g.Children.Add(vlcPlayer);
            Grid.SetRow(vlcPlayer, 1);
            vlcPlayer.MediaPlayer.BeginInit();
            vlcPlayer.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            vlcPlayer.MediaPlayer.VlcMediaplayerOptions = new string[] { "-I", "dummy", "--ignore-config", "--no-video-title" };
            vlcPlayer.MediaPlayer.EndInit();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);

            player.VolumeValue = vlcPlayer.MediaPlayer.Audio.Volume;
            player.StopEnabled = false;
            player.PauseEnabled = false;
            //End Player Settings

            //Restore default path after vlc initialization
            Directory.SetCurrentDirectory(startupPath);
        }

        private void OnVlcControlNeedsLibDirectory(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(Utils.GetWinVlcPath());
        }

        /*private void OnEndReached(object sender, Meta.Vlc.ObjectEventArgs<Meta.Vlc.Interop.Media.MediaState> e)
        {
            StopClick(sender);
        }*/
                
        /*public void PlayerOnVideoSourceChanged(object sender, VideoSourceChangedEventArgs videoSourceChangedEventArgs)
        {
            DisplayGrid.Dispatcher.BeginInvoke(new Action(() =>
            {
                DisplayGrid.Source = videoSourceChangedEventArgs.NewVideoSource;
            }));
        }*/

        private void timer_Tick(object sender, EventArgs e)
        {
            if (vlcPlayer.MediaPlayer.IsPlaying)
            {
                player.MovieProgress = vlcPlayer.MediaPlayer.Time;
                player.ProgressTime = TimeSpan.FromMilliseconds(player.MovieProgress).ToString(@"hh\:mm\:ss");
            }
        }

        public void UpdateVolumeChange()
        {
            vlcPlayer.MediaPlayer.Audio.Volume = player.VolumeValue;
        }

        public void UpdateMovieProgress()
        {
         //   player.MovieProgress = vlcPlayer.MediaPlayer.Time ;
         //   player.ProgressTime = TimeSpan.FromMilliseconds(player.MovieProgress).ToString(@"hh\:mm\:ss");
        }

        private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            player.VolumeValue = vlcPlayer.MediaPlayer.Audio.Volume += (e.Delta > 0) ? 1 : -1;
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void Terminate(object sender, EventArgs e)
        {
            vlcPlayer?.MediaPlayer.Stop();
            vlcPlayer?.Dispose();
        }
    }
}
