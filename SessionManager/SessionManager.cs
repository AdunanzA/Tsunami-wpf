using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tsunami.Core;
using NLog;

namespace Tsunami
{
    public static class SessionManager
    {
        private static Logger log = LogManager.GetLogger("SessionManager");
        private static Session _torrentSession = new Session();
        private static Timer _dispatcherTimer = new Timer();

        public static EventHandler<OnTorrentUpdatedEventArgs> TorrentUpdated;
        public static EventHandler<OnTorrentAddedEventArgs> TorrentAdded;
        public static EventHandler<OnTorrentRemovedEventArgs> TorrentRemoved;

        public static ConcurrentDictionary<string, Core.TorrentHandle> TorrentHandles = new ConcurrentDictionary<string, Core.TorrentHandle>();

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
            Settings.Logger.Inizialize();

            // http://www.libtorrent.org/reference-Alerts.html
            _torrentSession.set_alert_mask(AlertMask.all_categories);
            _torrentSession.set_alert_dispatch(HandleAlertCallback);
            
            _dispatcherTimer.Elapsed += new ElapsedEventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = Settings.Application.DISPATCHER_INTERVAL;
            _dispatcherTimer.Start();
            
        }

        public static void Terminate()
        {
            _dispatcherTimer.Stop();
            _torrentSession.clear_alert_dispatch();
            _torrentSession.Dispose();
        }

        private static void dispatcherTimer_Tick(object sender, ElapsedEventArgs e)
        {
            _torrentSession.post_torrent_updates();
        }

        private static void HandleAlertCallback(Alert a)
        {
            using (a)
            {
                // UPDATED
                if (a.GetType() == typeof(state_update_alert))
                {
                    OnTorrentUpdateAlert((state_update_alert)a);
                }
                // ADDED
                else if (a.GetType() == typeof(torrent_added_alert))
                {
                    torrent_added_alert taa = (torrent_added_alert)a;
                    log.Debug("torrent added: name {0}; status {1}; hash {2}", taa.handle.status().name, taa.handle.status().state.ToString(), taa.handle.status().info_hash.ToString());
                    OnTorrentAddAlert((torrent_added_alert)a);
                }
                // PAUSED
                else if (a.GetType() == typeof(torrent_paused_alert)) {
                    torrent_paused_alert tpa = (torrent_paused_alert)a;
                    log.Debug("torrent " + tpa.handle.status().name + " paused");
                }
                // RESUMED
                else if (a.GetType() == typeof(torrent_resumed_alert))
                {
                    torrent_resumed_alert tra = (torrent_resumed_alert)a;
                    log.Debug("torrent " + tra.handle.status().name + " resumed");
                }
                // REMOVED (torrent removed from torrentSession, always ok)
                else if (a.GetType() == typeof(torrent_removed_alert))
                {
                    torrent_removed_alert tra = (torrent_removed_alert)a;
                    TorrentRemoved?.Invoke(null, new OnTorrentRemovedEventArgs() { Hash = tra.handle.info_hash().ToString() });
                    log.Debug("torrent " + tra.handle.status().name + " removed");
                }
                // DELETED (finished deleting file for removed torrent)
                else if (a.GetType() == typeof(torrent_deleted_alert))
                {
                    torrent_deleted_alert tra = (torrent_deleted_alert)a;
                    log.Debug("files for torrent " + tra.handle.status().name + " deleted");
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
                    Hash = ts.info_hash.ToString(),
                    Name = ts.name,
                    Progress = ts.progress,
                    QueuePosition = ts.queue_position,
                    Status = Models.TorrentStatus.GiveMeStateFromEnum(ts.state)
                };
                //log.Trace("torrent: name {0}; status {1}; progress {2}", ts.name, ts.state.ToString(), ts.progress);
                TorrentUpdated?.Invoke(null, evnt);
            }
        }

        private static void OnTorrentAddAlert(torrent_added_alert a)
        {
            var th = a.handle;
            if (TorrentHandles.TryAdd(th.info_hash().ToString(), th))
            {
                var ts = th.status();
                var evnt = new OnTorrentAddedEventArgs
                {
                    Hash = th.info_hash().ToString(),
                    Name = ts.name,
                    Progress = ts.progress,
                    QueuePosition = ts.queue_position,
                    Status = Models.TorrentStatus.GiveMeStateFromEnum(ts.state)
                };
                //log.Debug("torrent added: name {0}; status {1}; hash {2}", ts.name, ts.state.ToString(), ts.info_hash.ToString());
                TorrentAdded?.Invoke(null, evnt);
            }
        }

        public class OnTorrentAddedEventArgs : EventArgs
        {
            public string Hash { get;set; }
            public int QueuePosition { get; set; }
            public string Name { get; set; }
            public float Progress { get; set; }
            public string Status { get; set; }
        }

        public class OnTorrentUpdatedEventArgs : EventArgs
        {
            public string Hash { get; set; }
            public int QueuePosition { get; set; }
            public string Name { get; set; }
            public float Progress { get; set; }
            public string Status { get; set; }
        }

        public class OnTorrentRemovedEventArgs : EventArgs
        {
            public string Hash { get; set; }
        }
    }
}
