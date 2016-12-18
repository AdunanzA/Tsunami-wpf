using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Tsunami.Pages.Dialogs
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            ver.Text = Settings.Application.TSUNAMI_VERSION;

        }

        private void GitHub_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Adunanza_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Forum_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        //private string GetTsunamiVersion()
        //{
        //    var _verMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
        //    var _verMin = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
        //    var _verRev = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
        //    string versionComplete = _verMajor + "." + _verMin + _verRev;

        //    return versionComplete;
        //}
    }
}
