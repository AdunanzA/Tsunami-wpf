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
using System.Windows.Shapes;

namespace Tsunami.Pages
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        public Loading()
        {
            InitializeComponent();
        }

        private bool closeCompleted = false;


        private void FormFadeOut_Completed(object sender, EventArgs e)
        {
            closeCompleted = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (!closeCompleted)
            {
                FormFadeOut.Begin();
                e.Cancel = true;
            }
        }
    }
}
