using System;
using System.Threading;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        FullScreen fullScreenWindow = null;
        public List<ColorItem> ColorsList = new List<ColorItem>();

        public MainWindow()
        {
            InitializeComponent();

            var vlcPath = Utils.GetWinVlcPath();
            if (vlcPath == null || !Directory.Exists(vlcPath))
            {
                var cd = new CustomMessage(string.Format("VLC" + "{0} bit" +  (string)FindResource("NotFound") + 
                    "Tsunami Streaming" + (string)FindResource("NotAvaiable"), Utils.Is64BitOs() ? "64" : "32"));
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

            Preferences pref = (Preferences)FindResource("Preferences");
            foreach (MahApps.Metro.Accent item in MahApps.Metro.ThemeManager.Accents)
            {
                System.Windows.Media.SolidColorBrush res = (System.Windows.Media.SolidColorBrush)item.Resources["HighlightBrush"];
                ColorItem ci = new ColorItem(res, item.Name.ToString());
                ColorsList.Add(ci);
                cmbColor.Items.Add(ci);
                if (pref.ThemeColor == ci.Name)
                {
                    cmbColor.SelectedIndex = cmbColor.Items.IndexOf(ci);//SelectedItem = typeof(ColorItem).GetProperty(ci.Name); //ci.Name;
                }
            }
            //cmbColor.SelectedItem = pref.ThemeColor;
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
            MetroTab.Dispatcher.Invoke(new Action(() => MetroTab.SelectedIndex = 1));
        }

        private void PauseTorrent_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button os = (System.Windows.Controls.Button)e.OriginalSource;
            TorrentItem ti = (TorrentItem)os.DataContext;
            Models.TorrentStatus status = SessionManager.Instance.getTorrentStatus(ti.Hash);
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
                AffirmativeButtonText = (string)FindResource("DelAffermativeButton"),
                NegativeButtonText = (string)FindResource("No"),
                FirstAuxiliaryButtonText = (string)FindResource("DelFirstAuxiliary"),
            };

            switch (await this.ShowMessageAsync("", (string)FindResource("DelTorrentMessage") + " " + ti.Name + " " + "?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, dialogSettings))
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

        private void confApply_Click(object sender, RoutedEventArgs e)
        {
            Preferences res = (Preferences)this.FindResource("Preferences");
            res.savePreferenceToFile();
            settingsFlyOut.IsOpen = false;
        }

        private void confCancel_Click(object sender, RoutedEventArgs e)
        {
            Preferences res = (Preferences)this.FindResource("Preferences");
            res.reloadPreferenceFromFile();
            res = (Preferences)FindResource("Preferences");
            //cmbColor.SelectedItem = res.ThemeColor;
            foreach (ColorItem ci in ColorsList)
            {
                if (ci.Name == res.ThemeColor)
                {
                    cmbColor.SelectedIndex = cmbColor.Items.IndexOf(ci);
                    break;
                }
            }
            settingsFlyOut.IsOpen = false;
        }

        private void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                Preferences res = (Preferences)this.FindResource("Preferences");
                res.PathDownload = fbd.SelectedPath;
            }
        }

        private void Chat_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discordapp.com/invite/0pfzTOXuEjt9ifvF");
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Preferences res = (Preferences)this.FindResource("Preferences");
            ColorItem ci = (ColorItem)cmbColor.SelectedItem;
            //res.ThemeColor = cmbColor.SelectedItem.ToString();
            res.ThemeColor = ci.Name;

            // devo fare refresh per tutti i grafici presenti in tutti i TorrentItem in download
            // altrimenti non cambiano colore
            TorrentStatusViewModel tsvm = (TorrentStatusViewModel)FindResource("TorrentStatusViewModel");
            foreach (TorrentItem ti in tsvm.TorrentList)
            {
                ti.CallPropertyChanged("ColorFrom");
                ti.CallPropertyChanged("ColorTo");
            }

        }

        private async void torrentFile_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Controls.TextBlock os = (System.Windows.Controls.TextBlock)e.OriginalSource;
            TorrentItem ti = (TorrentItem)os.DataContext;

            List<Models.FileEntry> feList = SessionManager.Instance.getTorrentFiles(ti.Hash);

            var dialogSettings = new MetroDialogSettings
            {
                SuppressDefaultResources = true,                
                AffirmativeButtonText = "Close",
            };

            string msg = ti.Name + " files:\n";

            foreach (Models.FileEntry fe in feList)
            {
                msg += "\n" + fe.FileName;
            }

            MessageDialogResult x = await this.ShowMessageAsync("", msg, MessageDialogStyle.Affirmative, dialogSettings);
        }
    }
}

public class ColorItem
{
    public System.Windows.Media.SolidColorBrush Color { get; set; }
    public string Name { get; set; }

    public ColorItem(System.Windows.Media.SolidColorBrush color, string name)
    {
        Color = color;
        Name = name;
    }
}