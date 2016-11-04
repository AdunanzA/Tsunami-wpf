using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Tsunami
{
    internal delegate void Invoker();

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            applicationInitialize = ApplicationInitialize;
        }
        public static new App Current
        {
            get { return Application.Current as App; }
        }
        internal delegate void ApplicationInitializeDelegate(ViewModel.TsunamiViewModel tsunamiVM);
        internal ApplicationInitializeDelegate applicationInitialize;
        private void ApplicationInitialize(ViewModel.TsunamiViewModel tsunamiVM)
        {
            // wait for tsunami fast resume
            while (!tsunamiVM.IsTsunamiEnabled)
            {
                Thread.Sleep(100);
            }

            // wait something else
            tsunamiVM.StringLoading = "Elaborating...";
            Thread.Sleep(2000);

            // Create the main window, but on the UI thread.
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                Current.MainWindow.Hide();
                Current.MainWindow = null;
                Current.MainWindow = new Tsunami.MainWindow();
                Current.MainWindow.Show();
            });
        }
    }
}
