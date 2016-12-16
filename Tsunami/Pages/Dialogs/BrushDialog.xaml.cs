using System.Windows;
using System.Windows.Controls;

namespace Tsunami.Pages.Dialogs
{
    /// <summary>
    /// Interaction logic for BrushDialog.xaml
    /// </summary>
    public partial class BrushDialog : UserControl
    {
        public BrushDialog()
        {
            InitializeComponent();
        }

        private void cmbColor_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Models.Preferences.ColorItem item in cmbColor.Items)
            {
                if (item.Name == Settings.User.ThemeColor)
                {
                    cmbColor.SelectedIndex = cmbColor.Items.IndexOf(item);
                    break;
                }
            }
        }

        private void cmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColor.SelectedItem != null)
            {
                ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
                Models.Preferences.ColorItem ci = (Models.Preferences.ColorItem)cmbColor.SelectedItem;
                tvm.Preference.ThemeColor = ci.Name;
            }
        }

        private void cmbAccent_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Models.Preferences.ColorItem item in cmbAccent.Items)
            {
                if (item.Name == Settings.User.AccentColor)
                {
                    cmbAccent.SelectedIndex = cmbAccent.Items.IndexOf(item);
                    break;
                }
            }
        }

        private void cmbAccent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAccent.SelectedItem != null)
            {
                ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
                Models.Preferences.ColorItem ci = (Models.Preferences.ColorItem)cmbAccent.SelectedItem;
                tvm.Preference.AccentColor = ci.Name;
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
