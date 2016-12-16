using System.Windows;
using System.Windows.Controls;

namespace Tsunami.Pages.Dialogs
{
    /// <summary>
    /// Interaction logic for WebSettingsDialog.xaml
    /// </summary>
    public partial class WebSettingsDialog : UserControl
    {
        public WebSettingsDialog()
        {
            InitializeComponent();
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
