using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Configuration;
using System.IO;
using Squirrel;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //private object aduDownload = new Downloads();
        string startupPath = System.IO.Directory.GetCurrentDirectory();
        //private object aduSearch = new Search();
        //private object aduPlayer = new Player();

        public MainWindow()
        {
            InitializeComponent();

            // If Nbug CrashReporting is Not Configured don't Inizialize it 
            if (System.Configuration.ConfigurationManager.AppSettings["NbugSmtpServer"] == "smtp.dummy.com") 
            {
                //Don't enable Nbug
            }
            else Initialize_CrashReporting();

            Directory.SetCurrentDirectory(startupPath);

            this.SetLanguageDictionary();
            var verMajor = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            var verMin = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            var verRev = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;
            var title = this.Title + " " +  + verMajor +  "." + verMin + verRev;
            this.Title = title;

            SessionManager.Initialize();
        }

        async static void SquirrellUpdate()
        {
            using (var mgr = new UpdateManager("C:\\Projects\\MyApp\\Releases"))
            {
                await mgr.UpdateApp();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionManager.Terminate();

            string str = "something to put in File";
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(string));
            var path = Environment.CurrentDirectory + "test.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, str);
            file.Close();

            //Save MainWindow Settings
            Properties.Settings.Default.Save();

        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {    
                case "en-US":
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "it-IT":
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../../../Resources/italian.xaml", UriKind.RelativeOrAbsolute);
                    break;

                default:
                    dict.Source = new Uri(Environment.CurrentDirectory+"../../../../Resources/english.xaml", UriKind.RelativeOrAbsolute);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        //page navigation methods
        //private void downloadsButton_Click(object sender, RoutedEventArgs e)
        //{
        //    PageContainer.Content = aduDownload; 
        //}

        //private void searchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    PageContainer.Content = aduSearch; 
        //}

        //private void playerButton_Click(object sender, RoutedEventArgs e)
        //{
        //    PageContainer.Content = aduPlayer; 
        //}
        
        private void Initialize_CrashReporting()
        {
            // TODO: Probabilmente dobbiamo rendere l'inizializzazione async
            // Uncomment the following after testing to see that NBug is working as configured
            //NBug.Settings.ReleaseMode = true;

            // NBug configuration (you can also choose to create xml configuration file)
            //NBug.Settings.StoragePath = NBug.Enums.StoragePath.IsolatedStorage;

            NBug.Settings.StoragePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            NBug.Settings.UIMode = NBug.Enums.UIMode.Full;
            var _smtpUser = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpUser"];
            var _smtpPass = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpPass"];
            var _smtpServer = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpServer"];
            var _smtpPort = System.Configuration.ConfigurationManager.AppSettings["NbugSmtpPort"];

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
            // es.: NBug.Settings.AddDestinationFromConnectionString("Type=Mail;From=bugs@xxx.com;Port=465;SmtpServer=smtp.gmail.com;To=support@xxx.com;UseAttachment=True;UseAuthentication=True;UseSsl=True;Username=bugs@xxx.com;Password=xxx;");

            // Hook-up to all possible unhandled exception sources for WPF app, after NBug is configured
            //AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
            //Application.Current.DispatcherUnhandledException += NBug.Handler.DispatcherUnhandledException;
        }

        private void showSettingFlyOut_Click(object sender, RoutedEventArgs e)
        {
            settingsFlyOut.IsOpen = true;
        }

        private void AddTorrent_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.DefaultExt = ".torrent";
            ofd.Filter = "Torrent|*.torrent";
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Select Torrent to Add";
            ofd.ShowDialog();
            foreach (string file in ofd.FileNames)
            {
                SessionManager.addTorrent(file);
            }

        }

        //private void ToggleSwitch_Click(object sender, RoutedEventArgs e)
        //{
        //    TorrentStatusViewModel res = (TorrentStatusViewModel) this.FindResource("TorrentStatusViewModel");
        //    if (res != null)
        //    {
        //        ToggleSwitch ts = (ToggleSwitch)sender;
        //        res.UserPreferences.ShowAdvancedInterface = ts.IsChecked.Value;
        //    }
        //}
    }
}
