using System;
using System.Threading;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Squirrel;
using System.ComponentModel;

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


            Streaming.StreamingManager.SetSurface?.Invoke(this, DisplayImage);

            fullScreenWindow = new FullScreen(this);

            Closing += Window_Closing;



            SetLanguageDictionary();
            var verMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            var verMin = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            var verRev = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
            var title = Title + " " + +verMajor + "." + verMin + verRev;
            Title = title;

            SessionManager.Initialize();
            SessionManager.LoadFastResumeData();
        }

        async static void SquirrellUpdate()
        {
            using (var mgr = new UpdateManager(@"C:\DHT\TsunamiLocal\GUI_WPF\bin\x64"))
            {
                await mgr.UpdateApp();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Hide();
            SessionManager.Terminate();


            /*string str = "something to put in File";
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(string));
            var path = Environment.CurrentDirectory + "test.xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, str);
            file.Close();*/

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

        private void PauseTorrent_Click(object sender, RoutedEventArgs e)
        {
            //Until Undone
        }
        private void DeleteTorrent_Click(object sender, RoutedEventArgs e)
        {
           // System.Windows.Controls.Button os = (System.Windows.Controls.Button)e.OriginalSource;
           // TorrentItem ti = (TorrentItem)os.DataContext;
           // SessionManager.deleteTorrent(ti.Hash, false);
           // torrentList.Items.RemoveAt(torrentList.Items.IndexOf(torrentList.SelectedItem));
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

        private void FullScreenClick(object sender, RoutedEventArgs e)
        {
            fullScreenWindow.SetFullScreen();
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
