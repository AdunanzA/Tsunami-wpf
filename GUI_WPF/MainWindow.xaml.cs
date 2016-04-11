using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using Microsoft.Win32;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<int, string, double> Torrentlist { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Torrentlist = new ObservableCollection<int,string,double>();
            this.DataContext = Torrentlist;
            this.SetLanguageDictionary();

            SessionManager.Initialize();
            SessionManager.TorrentUpdated += new EventHandler<SessionManager.OnTorrentUpdatedEventArgs>(UpdateFromTsunamiCore);
            SessionManager.TorrentAdded += new EventHandler<SessionManager.OnTorrentAddedEventArgs>(AddFromTsunamiCore);
            SessionManager.TorrentRemoved += new EventHandler<SessionManager.OnTorrentRemovedEventArgs>(RemovedFromTsunamiCore);
            SessionManager.SessionStatisticsUpdate += new EventHandler<SessionManager.OnSessionStatisticsEventArgs>(UpdateFromSessionStatistics);


            //dataGridx.ItemsSource = Torrentlist;
        }

        private void UpdateFromSessionStatistics(object sender, SessionManager.OnSessionStatisticsEventArgs e)
        {
            // notify web of new session statistics
            //var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
            //context.Clients.All.notifySessionStatistics(e);
        }

        private void RemovedFromTsunamiCore(object sender, SessionManager.OnTorrentRemovedEventArgs e)
        {

        }

        private void AddFromTsunamiCore(object sender, SessionManager.OnTorrentAddedEventArgs e)
        {
            // notify web that a new id must be requested via webapi
            //var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
            //context.Clients.All.notifyTorrentAdded(e.Hash);
        }

        private void UpdateFromTsunamiCore(object sender, SessionManager.OnTorrentUpdatedEventArgs e)
        {
            // update web
            //var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
            //context.Clients.All.notifyUpdateProgress(e);
        }

        private void AutoKill_Click(object sender, RoutedEventArgs e)
        {
            //Environment.Exit(0);
            //var set = new Settings();
            //set.PATH_DOWNLOAD = "ciao";
        }

        private void ReadXML_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Non toccare cio' che non comprendi!" + Environment.NewLine + "Feature in development!");
            //    var ds = new DataSet;
            //    ds.ReadXml(Environment.CurrentDirectory  + "/config-core.xml");
        }

        private void AddTorrent_Click(object sender, RoutedEventArgs e)
        {
            var df = new TextDataFormat();
            // se c'è un link negli appunti allora lo aggiungo ai download 
            // altrimenti chiedo di indicare un file torrent
            if (Clipboard.GetText(df).Contains("magnet"))
            {
                string message = "Aggiungere questo magnet ai downloads ? ";
                string caption = "Confirmation";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.OK)
                {
                    // ok aggiungo 
                    //TorSession.add_magnet(Clipboard.GetText(df));
                }
                else AddTorrent();
            }
            else
            {
                AddTorrent();
            }
        }

        public void AddTorrent()
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
            
            //Torrentlist = new ObservableCollection<int, string, double>();
            //SessionManager.TorrentSession.add_torrent(atp);
            //TorrentHandle[] th = SessionManager.TorrentSession.get_torrents();
            //foreach (TorrentHandle t in th)
            //{
            //    Torrentlist.Add(t.torrent_file().name());
            //}
            //dataGridx.ItemsSource = Torrentlist;
        }

        private void StartATorrent_Click(object sender, RoutedEventArgs e)
        {
            //TorSession.get_torrents();
            //SessionManager.TorrentSession.get_torrents();
        }

        public class ObservableCollection<T1, T2, T3> : ObservableCollection<string>
        {
        }

        //gobne start
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string str = "something to put in File";
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(string));
            var path = Environment.CurrentDirectory + "test.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, str);
            file.Close();
            SessionManager.Terminate();
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {    
                case "en-US":
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "it-IT":
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../../../Resources/italian.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
        //gobne end

        private void btnOpenWeb_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://"+Settings.User.WebAddress+":"+Settings.User.WebPort);
        }
    }

}
