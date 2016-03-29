using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tsunami.Core;

namespace Tsunami
{
    public static class SessionManager
    {
        private static Session _torrentSession = new Session();
        private static Timer _dispatcherTimer = new Timer();

        public static EventHandler<OnTorrentUpdatedEventArgs> TorrentUpdated;
        public static EventHandler<OnTorrentAddedEventArgs> TorrentAdded;

        public static Session TorrentSession
        {
            get
            {
                return _torrentSession;
            }

            set
            {
                _torrentSession = value;
            }
        }

        public static void Initialize()
        {
            // http://www.libtorrent.org/reference-Alerts.html
            _torrentSession.set_alert_mask(AlertMask.all_categories);
            _torrentSession.set_alert_dispatch(HandleAlertCallback);
            _dispatcherTimer.Elapsed += new ElapsedEventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = Settings.DISPATCHER_INTERVAL;
            _dispatcherTimer.Start();
        }

        private static void dispatcherTimer_Tick(object sender, ElapsedEventArgs e)
        {
            _torrentSession.post_torrent_updates();
        }

        private static void HandleAlertCallback(Alert a)
        {
            using (a)
            {
                if (a.GetType() == typeof(state_update_alert))
                {
                    OnTorrentUpdateAlert((state_update_alert)a);
                }
                else if (a.GetType() == typeof(torrent_added_alert))
                {
                    OnTorrentAddAlert((torrent_added_alert)a);
                }
            }
        }

        private static void OnTorrentUpdateAlert(state_update_alert a)
        {
            var torrentsStatus = a.status;
            foreach (TorrentStatus ts in torrentsStatus)
            {
                var evnt = new OnTorrentUpdatedEventArgs
                {
                    Name = ts.name,
                    Progress = ts.progress,
                    QueuePosition = ts.queue_position
                };
                TorrentUpdated?.Invoke(null, evnt);
            }
        }

        private static void OnTorrentAddAlert(torrent_added_alert a)
        {
            var th = a.handle;
            var ts = th.status();
            var evnt = new OnTorrentAddedEventArgs
            {
                Name = ts.name,
                Progress = ts.progress,
                QueuePosition = ts.queue_position
            };
            TorrentAdded?.Invoke(null, evnt);
        }

        public class OnTorrentAddedEventArgs : EventArgs
        {
            public int QueuePosition { get; set; }
            public string Name { get; set; }
            public float Progress { get; set; }
        }

        public class OnTorrentUpdatedEventArgs : EventArgs
        {
            public int QueuePosition { get; set; }
            public string Name { get; set; }
            public float Progress { get; set; }
        }
    }
}
