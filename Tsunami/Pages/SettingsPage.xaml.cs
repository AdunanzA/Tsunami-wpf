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
            var brushDiag = new Dialogs.BrushDialog();
            await MaterialDesignThemes.Wpf.DialogHost.Show(brushDiag, "RootDialog");
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var downloadDiag = new Dialogs.DownloadDialog();
            await MaterialDesignThemes.Wpf.DialogHost.Show(downloadDiag, "RootDialog");
        }

        private async void WebButton_Click(object sender, RoutedEventArgs e)
        {
            var webDiag = new Dialogs.WebSettingsDialog();
            await MaterialDesignThemes.Wpf.DialogHost.Show(webDiag, "RootDialog");
        }
    }
}
