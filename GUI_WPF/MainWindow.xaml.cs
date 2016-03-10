using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Session TorSession; 
        public MainWindow()
        {
            InitializeComponent();
           
        }
        
        private void AutoKill_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            //var set = new Settings();
            //set.PATH_DOWNLOAD = "ciao";
        }

        private void ReadXML_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Non toccare cio' che non comprendi!" +Environment.NewLine+"Feature in development!");
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
                // OK Aggiungo
            }
            else
            {
                    // Cancel code here Opening filedialog
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.DefaultExt=".torrent";
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.Title = "Select Torrent to Add";
                    ofd.ShowDialog();
            }
            
                //TorSession.add_torrent(Clipboard.GetText().First());
                //TorrentHandle[] th = TorSession.get_torrents();
                //List<String> a = new List<String>();
                //foreach (TorrentHandle t in th)
                //{
                //    a.Add((t.torrent_file()).name());
                //}

                //dataGridx.ItemsSource = a;

            }

        
        }

        private void StartATorrent_Click(object sender, RoutedEventArgs e)
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
        //gobne end
    }
}
