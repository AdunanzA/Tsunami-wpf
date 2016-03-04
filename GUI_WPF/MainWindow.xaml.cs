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

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            var s = new Session();
            TorrentHandle[] th = s.get_torrents();
            List<String> a = new List<String>();
            foreach (TorrentHandle t in th)
            {
                a.Add((t.torrent_file()).name());
            }
            dataGridx.ItemsSource = a;
        }
    }


}
