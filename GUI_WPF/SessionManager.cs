using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tsunami.Core;

namespace Tsunami.Gui.Wpf
{
    static class SessionManager
    {
        //private static Session _torrentSession = new Session();
        //private static Timer _dispatcherTimer = new Timer();

        //public static Session TorrentSession
        //{
        //    get
        //    {
        //        return _torrentSession;
        //    }

        //    set
        //    {
        //        _torrentSession = value;
        //    }
        //}

        //public static void Initialize()
        //{
        //    // http://www.libtorrent.org/reference-Alerts.html
        //    _torrentSession.set_alert_mask(AlertMask.all_categories);
        //    _torrentSession.set_alert_dispatch(HandleAlertCallback);
        //    _dispatcherTimer.Elapsed += new ElapsedEventHandler(dispatcherTimer_Tick);
        //    _dispatcherTimer.Interval = 500;
        //    _dispatcherTimer.Start();
        //}

        //private static void dispatcherTimer_Tick(object sender, ElapsedEventArgs e)
        //{
        //    _torrentSession.post_torrent_updates();
        //}

        //private static void HandleAlertCallback(Alert a)
        //{
        //    using (a)
        //    {
        //        if (a.GetType() == typeof(state_update_alert))
        //        {
        //            OnTorrentUpdateAlert((state_update_alert)a);
        //        }
        //    }
        //}

        //private static void OnTorrentUpdateAlert(state_update_alert a)
        //{
        //    var torrentsStatus = a.status;
        //    foreach (TorrentStatus ts in torrentsStatus)
        //    {
        //        /*var msg = new TorrentAddedMessage
        //        {
        //            Name = ts.name,
        //            Progress = ts.progress,
        //            QueuePosition = ts.queue_position
        //        };
        //        _eventAggregator.PublishOnBackgroundThread(msg);*/
        //        var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
        //        context.Clients.All.notifyUpdateProgress(ts.name, ts.progress);
        //    }
        //}
    }
}
