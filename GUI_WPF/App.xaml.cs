using System.Windows;
using System;
using System.Threading.Tasks;
using System.Linq;
using NLog;
using Squirrel;

namespace Tsunami.Gui.Wpf
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger log = LogManager.GetLogger("App");

        protected override void OnStartup(StartupEventArgs e)
        {

            Initialize_CrashReporting();
            SquirrelUpdate();

            WpfSingleInstance.Make();
            base.OnStartup(e);
        }

        private void Initialize_CrashReporting()
        {
            AppDomain.CurrentDomain.UnhandledException += NBug.Handler.UnhandledException;
            Current.DispatcherUnhandledException += NBug.Handler.DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += NBug.Handler.UnobservedTaskException;
        }


        async private void SquirrelUpdate()
        {
            try
            {
                var hash = Utils.GetSettings("external.config");
                var key = Utils.Is64BitOs() ? "x64" : "x86";
                if (hash.ContainsKey(key))
                {
                    var path = hash[key].ToString();
                    using (var mgr = new UpdateManager(path))
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
                else
                {
                    log.Trace("Please define update path.");
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex,"Not a squirrel update folder", ex.StackTrace);
            }
        }
    }
}

