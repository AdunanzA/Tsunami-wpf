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
using Microsoft.Win32;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per Downloads.xaml
    /// </summary>
    public partial class Downloads : Page
    {
        public Downloads()
        {
            InitializeComponent();
            this.SetLanguageDictionary();
        }

        private void btnOpenWeb_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + Settings.User.WebAddress + ":" + Settings.User.WebPort);
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

        private void StartATorrent_Click(object sender, RoutedEventArgs e)
        {

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
            //OpenFileDialog ofd = new OpenFileDialog();
            ////ofd.DefaultExt = ".torrent";
            //ofd.Filter = "Torrent|*.torrent";
            //ofd.Multiselect = true;
            //ofd.CheckFileExists = true;
            //ofd.CheckPathExists = true;
            //ofd.Title = "Select Torrent to Add";
            //ofd.ShowDialog();
            //foreach (string file in ofd.FileNames)
            //{
            //    SessionManager.addTorrent(file);
            //}
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri(Environment.CurrentDirectory + "../../../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "it-IT":
                    dict.Source = new Uri(Environment.CurrentDirectory + "../../../../Resources/italian.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    dict.Source = new Uri(Environment.CurrentDirectory + "../../../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
    }

}
