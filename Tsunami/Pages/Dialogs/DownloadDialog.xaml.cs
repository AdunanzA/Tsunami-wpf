using System.Windows;
using System.Windows.Controls;

namespace Tsunami.Pages.Dialogs
{
    /// <summary>
    /// Interaction logic for DownloadDialog.xaml
    /// </summary>
    public partial class DownloadDialog : UserControl
    {
        public DownloadDialog()
        {
            InitializeComponent();
        }

        private void DownloadFolder_Click(object sender, RoutedEventArgs e)
        {
            string path = Classes.Utils.RetrieveDirectory();
            if (!string.IsNullOrWhiteSpace(path))
            {
                ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
                tvm.Preference.PathDownload = path;
            }
        }

        private void confApply_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
            tvm.Preference.savePreferenceToFile();
        }

        private void confCancel_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
            tvm.Preference.reloadPreferenceFromFile();
        }
    }
}
