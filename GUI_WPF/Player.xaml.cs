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
using xZune.Vlc.Interop.MediaPlayer;
using xZune.Vlc.Wpf;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per Player.xaml
    /// </summary>
    public partial class Player : Page
    {
        VlcPlayer myPlayer = new VlcPlayer();
        public Player()
        {
            if (IntPtr.Size == 8)       // 64 bit - impostare il vs. path
                myPlayer.Initialize(@"C:\Program Files\VideoLAN\VLC", "--ignore-config");
            else if (IntPtr.Size == 4)  // 32 bit - impostare il vs. path
                myPlayer.Initialize(@"..\..\..\LibVlc", "--ignore-config");

            myPlayer.Background = Brushes.Black;

            Grid.SetRow(myPlayer, 0);
            Grid.SetColumn(myPlayer, 0);
            Grid.SetRowSpan(myPlayer, 1);

            InitializeComponent();
            myGrid.Children.Add(myPlayer);

            volumeControl.Value = myPlayer.Volume;
            Stop.IsEnabled = false;
            Pause.IsEnabled = false;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            myPlayer.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
            myPlayer.Play();
            myPlayer.Stretch = Stretch.Fill;
            myPlayer.StretchDirection = StretchDirection.Both;
            Play.IsEnabled = false;
            Pause.IsEnabled = true;
            Stop.IsEnabled = true;
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            myPlayer.PauseOrResume();
           
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            myPlayer.Stop();
            Stop.IsEnabled = false;
            Pause.IsEnabled = false;
            Play.IsEnabled = true;

        }

        private void manageVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            myPlayer.Volume = (int)volumeControl.Value;
        }
    }
}
