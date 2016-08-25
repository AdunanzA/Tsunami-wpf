using System.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // If Nbug CrashReporting is Not Configured don't Inizialize it 
            if (Wpf.Properties.Settings.Default.NbugSmtpServer != "smtp.dummy.com")
                Initialize_CrashReporting();

            WpfSingleInstance.Make();

            base.OnStartup(e);
        }

        private void Initialize_CrashReporting()
        {
            // Uncomment the following after testing to see that NBug is working as configured
            NBug.Settings.ReleaseMode = true;

            NBug.Settings.StoragePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            NBug.Settings.UIMode = NBug.Enums.UIMode.Full;
            var _smtpUser = Wpf.Properties.Settings.Default.NbugSmtpUser;
            var _smtpPass = Wpf.Properties.Settings.Default.NbugSmtpPass;
            var _smtpServer = Wpf.Properties.Settings.Default.NbugSmtpServer;
            var _smtpPort = Wpf.Properties.Settings.Default.NbugSmtpPort;

            // Only one line connection-string no space & no SSL :(
            NBug.Settings.AddDestinationFromConnectionString(
                "Type=Mail;"
                + "From=tsunami-bugs@adunanza.net;"
                + "Port=" + _smtpPort + ";"
                + "SmtpServer=" + _smtpServer + ";"
                + "To=devteam@adunanza.net;"
                + "UseAttachment=True;"
                + "UseAuthentication=True;"
                + "UseSsl=False;"
                + "Username=" + _smtpUser + ";"
                + "Password=" + _smtpPass + ";"
                );
            
            // Hook-up to all possible unhandled exception sources for WPF app, after NBug is configured
            AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
            Application.Current.DispatcherUnhandledException += NBug.Handler.DispatcherUnhandledException;
        }
    }
}

