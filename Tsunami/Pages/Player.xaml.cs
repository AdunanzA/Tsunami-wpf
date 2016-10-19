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
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        FullScreen fullScreenWindow;

        public Player()
        {
            InitializeComponent();

            var vlcPath = Classes.Utils.GetWinVlcPath();
            if (vlcPath == null || System.IO.Directory.Exists(vlcPath))
            {
                //string msg = string.Format("VLC {0} bit {1}. Tsunami Streaming {2}", Utils.Is64BitOs() ? "64" : "32",
                //                                                                     (string)FindResource("NotFound"),
                //                                                                     (string)FindResource("NotAvaiable"));
                //DialogHost dh = new DialogHost() { HorizontalAlignment = HorizontalAlignment.Center,
                //                                   VerticalAlignment = VerticalAlignment.Center,
                //                                   Name = "Warning" };
                //dh.Content = msg;
                //dh.ShowDialog(msg).ContinueWith(t => t.Result);

                //Streaming.StreamingManager.SetPauseButtonStatus = null;
                //Streaming.StreamingManager.SetPlayButtonStatus = null;
                //Streaming.StreamingManager.SetStopButtonStatus = null;
            }
            else
            {
                //Streaming.StreamingManager.SetSurface?.Invoke(this, playerGrid);
            }
            fullScreenWindow = new FullScreen((Tsunami.MainWindow)Application.Current.MainWindow);

        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            fullScreenWindow.SetFullScreen();
        }



    }
}
