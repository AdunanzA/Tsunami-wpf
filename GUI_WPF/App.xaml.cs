using System.Windows;
using System.Threading;
using System;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WpfSingleInstance.Make();

            base.OnStartup(e);
        }
    }
}

