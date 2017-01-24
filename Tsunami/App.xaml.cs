using System.Threading;
using System.Windows;
using System.Windows.Threading;
using NLog;
using Squirrel;
using System;
using System.Linq;

namespace Tsunami
{
    internal delegate void Invoker();

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger log = LogManager.GetLogger("App");

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
                Thread.Sleep(500);
            }

            // wait something else
            tsunamiVM.StringLoading = "Elaborating...";
            Thread.Sleep(2000);

            // Create the main window, but on the UI thread.
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
            {
                //Current.MainWindow.Hide();
                Current.MainWindow.Close();
                Current.MainWindow = null;
                Current.MainWindow = new Tsunami.MainWindow();
                Current.MainWindow.Show();
                SquirrelUpdate();
            });
        }

        async private void SquirrelUpdate()
        {
            try
            {
                //var hash = Classes.Utils.GetSettings("external.config");
                var key = Classes.Utils.Is64BitOs() ? "x64" : "x86";
                //if (hash.ContainsKey(key))
                if(true) //temporary
                {
                    //var path = hash[key].ToString();
                    using (var mgr = new UpdateManager("ftp://do.adunanza.net/"))
                    {
                        var updates = mgr.CheckForUpdate();
                        await updates;

                        if (updates.Result.ReleasesToApply.Any())
                        {
                            var latestVersion = updates.Result.ReleasesToApply.OrderBy(x => x.Version).Last();
                            log.Trace("There is an update available");
                            try
                            {
                                await mgr.DownloadReleases(updates.Result.ReleasesToApply);
                            }
                            catch (Exception)
                            {
                                log.Trace("There was an error trying to download updates.");
                            }
                            try
                            {
                                await mgr.ApplyReleases(updates.Result);
                            }
                            catch (Exception)
                            {
                                log.Trace("There was an error trying to apply updates.");
                            }
                            try
                            {
                                await mgr.CreateUninstallerRegistryEntry();
                            }
                            catch (Exception)
                            {
                                log.Trace("There was an error trying to create the relevant registry entries for the update. " +
                                "Please try reinstalling the latest full version of the application.");
                            }
                        }
                        else
                        {
                            log.Trace("No updates available.");
                        }
                    }
                }
                //else
                //{
                //    log.Trace("Please define update path.");
                //}
            }
            catch (Exception ex)
            {
                log.Warn(ex, "Not a squirrel update folder", ex.StackTrace);
            }
        }
    }
}
