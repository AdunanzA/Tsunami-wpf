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
    /// Interaction logic for TSUCard.xaml
    /// </summary>
    public partial class TSUCard : UserControl
    {
        public TSUCard()
        {
            InitializeComponent();
        }

        private async void torrentFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock or = (TextBlock)e.OriginalSource;
            Models.TorrentItem ti = (Models.TorrentItem)or.DataContext;
            var sampleMessageDialog = new FileList
            {
                DataContext = ti
            };
            await MaterialDesignThemes.Wpf.DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Image os = (Image)e.OriginalSource;
            Models.TorrentItem ti = (Models.TorrentItem)os.DataContext;
            TsunamiViewModel res = (TsunamiViewModel)FindResource("TsunamiVM");
            res.PauseTorrent(ti.Hash);
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            Image os = (Image)e.OriginalSource;
            Models.TorrentItem ti = (Models.TorrentItem)os.DataContext;
            TsunamiViewModel res = (TsunamiViewModel)FindResource("TsunamiVM");
            res.ResumeTorrent(ti.Hash);
        }

        private void btnCancel_Click(object sender, MouseButtonEventArgs e)
        {
            FormFadeOut.Begin();
            //Button os = (Button)e.OriginalSource;
            //MaterialDesignThemes.Wpf.PackIcon os = (MaterialDesignThemes.Wpf.PackIcon)e.Source;
            //Models.TorrentItem ti = (Models.TorrentItem)os.DataContext;
            //TsunamiViewModel res = (TsunamiViewModel)FindResource("TsunamiVM");
            //res.RemoveTorrent(ti.Hash);
        }

        private void FormFadeOut_Completed(object sender, EventArgs e)
        {
            if (DataContext is Models.TorrentItem)
            {
                Models.TorrentItem ti = (Models.TorrentItem)DataContext;
                TsunamiViewModel res = (TsunamiViewModel)FindResource("TsunamiVM");
                res.RemoveTorrent(ti.Hash);
            }
        }
    }
}
