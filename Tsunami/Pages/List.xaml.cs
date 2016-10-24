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
using Tsunami.ViewModel;

namespace Tsunami.Pages
{
    /// <summary>
    /// Interaction logic for List.xaml
    /// </summary>
    public partial class List : UserControl
    {
        public List()
        {
            InitializeComponent();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Button os = (Button)e.OriginalSource;
            Models.TorrentItem ti = (Models.TorrentItem)os.DataContext;
            TsunamiViewModel res = (TsunamiViewModel)FindResource("TsunamiVM");
            res.PauseTorrent(ti.Hash);
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            Button os = (Button)e.OriginalSource;
            Models.TorrentItem ti = (Models.TorrentItem)os.DataContext;
            TsunamiViewModel res = (TsunamiViewModel)FindResource("TsunamiVM");
            res.ResumeTorrent(ti.Hash);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Button os = (Button)e.OriginalSource;
            Models.TorrentItem ti = (Models.TorrentItem)os.DataContext;
            TsunamiViewModel res = (TsunamiViewModel)FindResource("TsunamiVM");
            res.RemoveTorrent(ti.Hash);
        }

    }
}
