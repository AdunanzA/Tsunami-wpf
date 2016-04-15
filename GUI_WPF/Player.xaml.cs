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

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per Player.xaml
    /// </summary>
    public partial class Player : Page
    {

        public Player()
        {            
            InitializeComponent();
            volumeControl.Value = myPlayer.Volume;
            Stop.IsEnabled = false;
            Pause.IsEnabled = false;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            myPlayer.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
            myPlayer.Play();
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
            myPlayer.Dispose(); 
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
