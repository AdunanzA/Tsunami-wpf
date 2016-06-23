using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Configuration;
using System.IO;
using Squirrel;
using System.Windows.Threading;
using Meta.Vlc.Wpf;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string startupPath = System.IO.Directory.GetCurrentDirectory();

        DispatcherTimer timer = null;
        DispatcherTimer hideBarTimer = null;

        bool isFullScreen = false;
        bool isDragging = false;
        VlcPlayer vlcPlayer = null;
        Window fscreen = null;
        Grid fscreenGrid = null;
        public MainWindow()
        {
            InitializeComponent();

            // If Nbug CrashReporting is Not Configured don't Inizialize it 
            if (System.Configuration.ConfigurationManager.AppSettings["NbugSmtpServer"] == "smtp.dummy.com") 
            {
                //Don't enable Nbug
            }
            else Initialize_CrashReporting();

            //Player Settings
            var vlcPath = Utils.GetWinVlcPath();

            if (Utils.IsWindowsOs())
            {
                Directory.SetCurrentDirectory(vlcPath);
            }                        

            vlcPlayer = new VlcPlayer(DisplayImage.Dispatcher);
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

            volumeControl.Value = vlcPlayer.Volume;
            Stop.IsEnabled = false;
            Pause.IsEnabled = false;
            //End Player Settings

            //Restore default path after vlc initialization
            Directory.SetCurrentDirectory(startupPath);

            this.SetLanguageDictionary();
            var verMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            var verMin = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            var verRev = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;
            var title = this.Title + " " +  + verMajor +  "." + verMin + verRev;
            this.Title = title;

            SessionManager.Initialize();
        }

        async static void SquirrellUpdate()
        {
            using (var mgr = new UpdateManager("C:\\Projects\\MyApp\\Releases"))
            {
                await mgr.UpdateApp();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionManager.Terminate();

            string str = "something to put in File";
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(string));
            var path = Environment.CurrentDirectory + "test.xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, str);
            file.Close();

            //Save MainWindow Settings
            Properties.Settings.Default.Save();

        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {    
                case "en-US":
                    dict.Source = new Uri(Environment.CurrentDirectory+ "/Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "it-IT":
                    dict.Source = new Uri(Environment.CurrentDirectory+ "/Resources/italian.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    dict.Source = new Uri(Environment.CurrentDirectory+ "/Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
        
        private void Initialize_CrashReporting()
        {
            // TODO: Probabilmente dobbiamo rendere l'inizializzazione async
            // Uncomment the following after testing to see that NBug is working as configured
            //NBug.Settings.ReleaseMode = true;

            // NBug configuration (you can also choose to create xml configuration file)
            //NBug.Settings.StoragePath = NBug.Enums.StoragePath.IsolatedStorage;

            NBug.Settings.StoragePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            NBug.Settings.UIMode = NBug.Enums.UIMode.Full;
            var _smtpUser = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpUser"];
            var _smtpPass = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpPass"];
            var _smtpServer = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpServer"];
            var _smtpPort = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpPort"];

            // Only one line connection-string no space & no SSL :(
            NBug.Settings.AddDestinationFromConnectionString(
                "Type=Mail;"
                + "From=tsunami-bugs@adunanza.net;"
                + "Port=" + _smtpPort + ";"
                + "SmtpServer=" + _smtpServer + ";"
                + "To=devteam@adunanza.net;"
                + "UseAttachment=True;"
                + "UseAuthentication=True;"
                + "UseSsl=False;"
                + "Username=" + _smtpUser + ";"
                + "Password=" + _smtpPass + ";"
                );
            // es.: NBug.Settings.AddDestinationFromConnectionString("Type=Mail;From=bugs@xxx.com;Port=465;SmtpServer=smtp.gmail.com;To=support@xxx.com;UseAttachment=True;UseAuthentication=True;UseSsl=True;Username=bugs@xxx.com;Password=xxx;");

            // Hook-up to all possible unhandled exception sources for WPF app, after NBug is configured
            //AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
            //Application.Current.DispatcherUnhandledException += NBug.Handler.DispatcherUnhandledException;
        }

        private void showSettingFlyOut_Click(object sender, RoutedEventArgs e)
        {
            settingsFlyOut.IsOpen = true;
        }

        private void AddTorrent_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.DefaultExt = ".torrent";
            ofd.Filter = "Torrent|*.torrent";
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Select Torrent to Add";
            ofd.ShowDialog();
            foreach (string file in ofd.FileNames)
            {
                SessionManager.addTorrent(file);
            }

        }

        private void showProgressBar(object sender, MouseEventArgs e)
        {
            if (isFullScreen)
            {
                playerStatus.Visibility = Visibility.Visible;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging && vlcPlayer.VideoSource != null)
            {
                movieProgress.Minimum = 0;
                movieProgress.Maximum = vlcPlayer.Length.TotalSeconds;
                movieProgress.Value = vlcPlayer.Time.TotalSeconds;
            }
        }

        private void HideBar_Tick(object sender, EventArgs e)
        {
            if (isFullScreen)
            {
                playerStatus.Visibility = Visibility.Collapsed;
                Mouse.OverrideCursor = Cursors.None;
            }
        }

        private void PlayerOnVideoSourceChanged(object sender, VideoSourceChangedEventArgs videoSourceChangedEventArgs)
        {

            DisplayImage.Dispatcher.BeginInvoke(new Action(() =>
            {
                    DisplayImage.Source = videoSourceChangedEventArgs.NewVideoSource;             
            }));
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            vlcPlayer.Stop();
            vlcPlayer.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));

            vlcPlayer.Play();
            timer.Start();
            hideBarTimer.Start();

            FullScreen.IsEnabled = true;
            Play.IsEnabled = false;
            Pause.IsEnabled = true;
            Stop.IsEnabled = true;
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            vlcPlayer.PauseOrResume();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            hideBarTimer.Stop();

            System.Threading.Tasks.Task.Run(() =>
            {
                vlcPlayer.Stop();
            });

            if (isFullScreen)
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
            Play.IsEnabled = true;
        }

        private void manageVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            vlcPlayer.Volume = (int)volumeControl.Value;
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(movieProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            volumeControl.Value = vlcPlayer.Volume += (e.Delta > 0) ? 1 : -1;
        }

        private void movieProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void movieProgrss_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            vlcPlayer.Time = TimeSpan.FromSeconds(movieProgress.Value);
        }

        private void SetFullScreen(object sender, EventArgs e)
        {

            if (!isFullScreen)
            {
                this.Hide();
                fscreen = new Window();
                fscreenGrid = new Grid();
                myGrid.Children.Remove(DisplayImage);
                myGrid.Children.Remove(playerStatus);
                fscreenGrid.Children.Add(DisplayImage);
                fscreenGrid.Children.Add(playerStatus);
                fscreen.Content = fscreenGrid; 
                fscreen.WindowState = WindowState.Maximized;
                fscreen.WindowStyle = WindowStyle.None;
                fscreen.Show();

                isFullScreen = true;
            }

            else
            {
                fscreenGrid.Children.Clear();
                myGrid.Children.Add(DisplayImage);
                myGrid.Children.Add(playerStatus);
                myGrid.SetValue(Canvas.ZIndexProperty, 10);
                vlcPlayer.SetValue(Canvas.ZIndexProperty, 11);
                fscreenGrid = null;
                fscreen.Close();
                fscreen = null;
                this.Show();                
                Mouse.OverrideCursor = Cursors.Arrow;
                isFullScreen = false;
            }
        }

        //private void ToggleSwitch_Click(object sender, RoutedEventArgs e)
        //{
        //    TorrentStatusViewModel res = (TorrentStatusViewModel) this.FindResource("TorrentStatusViewModel");
        //    if (res != null)
        //    {
        //        ToggleSwitch ts = (ToggleSwitch)sender;
        //        res.UserPreferences.ShowAdvancedInterface = ts.IsChecked.Value;
        //    }
        //}
    }
}
