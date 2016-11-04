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
using System.Windows.Threading;

namespace Tsunami.Pages
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        ViewModel.TsunamiViewModel tvm;

        public Loading()
        {
            InitializeComponent();
            Loaded += Splash_Loaded;
            tvm = (ViewModel.TsunamiViewModel)FindResource("TsunamiVM");
        }

        void Splash_Loaded(object sender, RoutedEventArgs e)
        {
            IAsyncResult result = null;

            // This is an anonymous delegate that will be called when the initialization has COMPLETED
            AsyncCallback initCompleted = delegate (IAsyncResult ar)
            {
                App.Current.applicationInitialize.EndInvoke(result);

                // Ensure we call close on the UI Thread.
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { Close(); });
            };

            // This starts the initialization process on the Application
            result = App.Current.applicationInitialize.BeginInvoke(tvm, initCompleted, null);
        }

        //public void SetProgress(double progress)
        //{
        //    // Ensure we update on the UI Thread.
        //    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate { progressLoading.Value = progress; });
        //}

        private bool closeCompleted;

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
