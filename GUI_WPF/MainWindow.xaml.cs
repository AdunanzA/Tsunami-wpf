using System;
using System.Threading;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        FullScreen fullScreenWindow = null;

        public MainWindow()
        {
            InitializeComponent();

            var vlcPath = Utils.GetWinVlcPath();
            if (vlcPath == null || !Directory.Exists(vlcPath))
            {
                var cd = new CustomMessage(string.Format("VLC {0} bit non trovato!!! Tsunami Streaming non disponibile!!!", Utils.Is64BitOs() ? "64" : "32"));
                {
                    Streaming.StreamingManager.SetPauseButtonStatus?.Invoke(this, false);
                    Streaming.StreamingManager.SetPlayButtonStatus?.Invoke(this, false);
                    Streaming.StreamingManager.SetStopButtonStatus?.Invoke(this, false);
                }
            }
            else
            {
                Streaming.StreamingManager.SetSurface?.Invoke(this, playerGrid);
            }

            fullScreenWindow = new FullScreen(this);

            Closing += Window_Closing;

            SetLanguageDictionary();
            var verMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            var verMin = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            var verRev = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
            var title = Title + " " + +verMajor + "." + verMin + verRev;
            Title = title;

            SessionManager.Instance.Initialize();
            SessionManager.Instance.LoadFastResumeData();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Hide();
            SessionManager.Instance.Terminate();

            //Save MainWindow Settings
            Properties.Settings.Default.Save();
            fullScreenWindow.Dispose();
            Streaming.StreamingManager.Terminate?.Invoke(this, null);

            Environment.Exit(0);
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri(Environment.CurrentDirectory + "/Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "it-IT":
                    dict.Source = new Uri(Environment.CurrentDirectory + "/Resources/italian.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    dict.Source = new Uri(Environment.CurrentDirectory + "/Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }



        private void showSettingFlyOut_Click(object sender, RoutedEventArgs e)
        {
            settingsFlyOut.IsOpen = true;
        }

        private void StreamTorrent_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button os = (System.Windows.Controls.Button)e.OriginalSource;
            TorrentItem ti = os.DataContext as TorrentItem;
            string path = SessionManager.Instance.GetFilePathFromHash(ti.Hash, 0);

            if (CheckVideoExts(Path.GetExtension(path)) == false)
            {
                this.ShowMessageAsync("Error", "Streaming not available!", MessageDialogStyle.Affirmative, null);
                return;
            }

            var status = SessionManager.Instance.getTorrentStatus(ti.Hash);
            if (!status.Paused && !status.IsSeeding)
            {
                SessionManager.Instance.BufferingCompleted = BufferingCompleted;
                SessionManager.Instance.StreamTorrent(ti.Hash, 0);
            }

        }

        private void BufferingCompleted(object sender, string path)
        {
            Tsunami.Streaming.StreamingManager.PlayMediaPath?.Invoke(this, path);
            MetroTab.Dispatcher.Invoke(new Action(() => MetroTab.SelectedIndex = 2));
        }



        private void PauseTorrent_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button os = (System.Windows.Controls.Button)e.OriginalSource;
            TorrentItem ti = (TorrentItem)os.DataContext;
            var status = SessionManager.Instance.getTorrentStatus(ti.Hash);
                if (!status.Paused)
                {
                    SessionManager.Instance.pauseTorrent(ti.Hash);
                }
                else
                {
                    SessionManager.Instance.resumeTorrent(ti.Hash);
                }
        }

        private async void DeleteTorrent_Click(object sender, RoutedEventArgs e)
        {
            TorrentStatusViewModel res = (TorrentStatusViewModel)this.FindResource("TorrentStatusViewModel");
            System.Windows.Controls.Button os = (System.Windows.Controls.Button)e.OriginalSource;
            TorrentItem ti = (TorrentItem)os.DataContext;
            var dialogSettings = new MetroDialogSettings
            {
                SuppressDefaultResources = true,
                AffirmativeButtonText = "Yes, and delete file from disk too",
                NegativeButtonText = "No",
                FirstAuxiliaryButtonText = "Yes, but keep file on disk",
            };

            switch (await this.ShowMessageAsync("", "Do you really want to delete" + " " + ti.Name + " " + "?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, dialogSettings))
            {
                case MessageDialogResult.Affirmative:
                    SessionManager.Instance.deleteTorrent(ti.Hash, true);
                    res.TorrentList.Remove(ti);
                    torrentList.Items.Refresh();
                    break;
                case MessageDialogResult.Negative:
                    break;
                case MessageDialogResult.FirstAuxiliary:
                    SessionManager.Instance.deleteTorrent(ti.Hash, false);
                    res.TorrentList.Remove(ti);
                    torrentList.Items.Refresh();
                    break;
                default:
                    break;
            }

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
                SessionManager.Instance.addTorrent(file);
            }

        }

        private void FullScreenClick(object sender, RoutedEventArgs e)
        {
            fullScreenWindow.SetFullScreen();
        }

        private void HandleDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    SessionManager.Instance.addTorrent(file);
                }
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

        private bool CheckVideoExts(string s)
        {
            int i = 0;

            s = s.ToUpper();
            string[] videoExts = { ".AVI", ".MKV", ".FLV", ".MOV", ".MP4", ".MPG", ".WMV", ".WEBM" };

            while (i < videoExts.Length)
            {
                if (String.Equals(s, videoExts[i++]))
                    return true;
            }
            return false;
        }

    }
}
