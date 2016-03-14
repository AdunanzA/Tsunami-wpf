using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Tsunami.Core;
using Microsoft.Win32;
using System.IO;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Session TorSession;
        public ObservableCollection<string> Torrentlist { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
<<<<<<< HEAD
            TorSession = new Session();
            Torrentlist = new ObservableCollection<int,string,double>();
            this.DataContext = Torrentlist;
=======
            this.SetLanguageDictionary();
>>>>>>> LANGUAGE
        }

        private void AutoKill_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
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
            // Cancel code here Opening filedialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".torrent";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Select Torrent to Add";
            ofd.ShowDialog();
            var atp = new AddTorrentParams();
            TorrentInfo ti = new TorrentInfo(ofd.FileName);
            atp.save_path = Environment.CurrentDirectory;
            atp.ti = ti;
            
            TorSession.add_torrent(atp);
            TorrentHandle[] th = TorSession.get_torrents();
            foreach (TorrentHandle t in th)
            {
                Torrentlist.Add(t.torrent_file().name());
            }
            dataGridx.ItemsSource = Torrentlist;
        }

        private void StartATorrent_Click(object sender, RoutedEventArgs e)
        {
            TorSession.get_torrents();
        }

        private class ObservableCollection<T1, T2, T3> : ObservableCollection<string>
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
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {    
                case "en-US":
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "it-IT":
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../../Resources/italian.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
        //gobne end
    }
}
