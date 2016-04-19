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


        public class ObservableCollection<T1, T2, T3> : ObservableCollection<string>
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string str = "something to put in File";
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(string));
            var path = Environment.CurrentDirectory + "test.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, str);
            file.Close();

            //Save MainWindow Settings
            Properties.Settings.Default.Save();

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

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        //page navigation methods
        private void downloadsButton_Click(object sender, RoutedEventArgs e)
        {
            PageContainer.Content = new Downloads();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            PageContainer.Content = new Search();
        }

        private void playerButton_Click(object sender, RoutedEventArgs e)
        {
            PageContainer.Content = new Player();
        }

        private void sharingButton_Click(object sender, RoutedEventArgs e)
        {
            PageContainer.Content = new Sharing();
        }


    }

}
