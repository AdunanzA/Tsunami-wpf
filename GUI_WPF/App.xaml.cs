using System.Windows;
using System;
using System.Threading.Tasks;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            Initialize_CrashReporting();

            WpfSingleInstance.Make();

            base.OnStartup(e);
        }

        private void Initialize_CrashReporting()
        {
            AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
            Current.DispatcherUnhandledException += NBug.Handler.DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += NBug.Handler.UnobservedTaskException;
        }
    }
}

