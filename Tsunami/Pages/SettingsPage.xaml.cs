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

namespace Tsunami.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void BrushButton_Click(object sender, RoutedEventArgs e)
        {
            var brushDiag = new BrushDialog();
            await MaterialDesignThemes.Wpf.DialogHost.Show(brushDiag, "RootDialog");
        }

        //private void selectFolder_Click(object sender, RoutedEventArgs e)
        //{
        //    string path = Classes.Utils.RetrieveDirectory();
        //    if (!string.IsNullOrWhiteSpace(path))
        //    {
        //        ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
        //        tvm.Preference.PathDownload = path;
        //    }
        //}
        //private void cmbColor_Loaded(object sender, RoutedEventArgs e)
        //{
        //    foreach (Models.Preferences.ColorItem item in cmbColor.Items)
        //    {
        //        if (item.Name == Settings.User.ThemeColor)
        //        {
        //            cmbColor.SelectedIndex = cmbColor.Items.IndexOf(item);
        //            break;
        //        }
        //    }
        //}

        //private void cmbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbColor.SelectedItem != null)
        //    {
        //        ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
        //        Models.Preferences.ColorItem ci = (Models.Preferences.ColorItem)cmbColor.SelectedItem;
        //        tvm.Preference.ThemeColor = ci.Name;
        //    }
        //}

        //private void cmbAccent_Loaded(object sender, RoutedEventArgs e)
        //{
        //    foreach (Models.Preferences.ColorItem item in cmbAccent.Items)
        //    {
        //        if (item.Name == Settings.User.AccentColor)
        //        {
        //            cmbAccent.SelectedIndex = cmbAccent.Items.IndexOf(item);
        //            break;
        //        }
        //    }
        //}

        //private void cmbAccent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbAccent.SelectedItem != null)
        //    {
        //        ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
        //        Models.Preferences.ColorItem ci = (Models.Preferences.ColorItem)cmbAccent.SelectedItem;
        //        tvm.Preference.AccentColor = ci.Name;
        //    }
        //}

        //private void confApply_Click(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
        //    tvm.Preference.savePreferenceToFile();
        //}

        //private void confCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.TsunamiViewModel tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
        //    tvm.Preference.reloadPreferenceFromFile();
        //}

    }
}
