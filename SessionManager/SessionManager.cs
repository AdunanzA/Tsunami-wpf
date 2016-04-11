using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NLog;
using Microsoft.Owin.Hosting;
//using Microsoft.Owin;

//[assembly: OwinStartup(typeof(Tsunami.www.Startup))]

namespace Tsunami
{
    public static class SessionManager
    {
        private static Logger log = LogManager.GetLogger("SessionManager");
        private static Core.Session _torrentSession = new Core.Session();
        private static Timer _dispatcherTimer = new Timer();

        public static EventHandler<OnTorrentUpdatedEventArgs> TorrentUpdated;
        public static EventHandler<OnTorrentAddedEventArgs> TorrentAdded;
        public static EventHandler<OnTorrentRemovedEventArgs> TorrentRemoved;
        public static EventHandler<OnSessionStatisticsEventArgs> SessionStatisticsUpdate;

        private static ConcurrentDictionary<string, Core.TorrentHandle> TorrentHandles = new ConcurrentDictionary<string, Core.TorrentHandle>();
        private static Dictionary<Type, Action<Object>> Alert2Func = new Dictionary<Type, Action<Object>>();

        private static Core.Session TorrentSession
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

            Uri baseAddress = new Uri(string.Format("http://{0}:{1}", Settings.User.WebAddress, Settings.User.WebPort));

            // web server
            //var props = new Dictionary<string, object>();
            //var address = Microsoft.Owin.BuilderProperties.Address.Create();
            //address.Host = baseAddress.Host;
            //address.Port = baseAddress.Port.ToString();
            //address.Scheme = baseAddress.Scheme;
            //address.Path = baseAddress.AbsolutePath;

            //props["host.Addresses"] = new List<IDictionary<string, object>> { address.Dictionary };
            //Microsoft.Owin.Host.HttpListener.OwinServerFactory.Initialize(props);
            //Microsoft.Owin.Host.HttpListener.OwinServerFactory.Create(new Func<IDictionary<string, object>, Task>, props);
            WebApp.Start<www.Startup>(baseAddress.ToString());

            // http://www.libtorrent.org/reference-Alerts.html
            _torrentSession.set_alert_mask(Core.AlertMask.all_categories);
            _torrentSession.set_alert_dispatch(HandleAlertCallback);

            Alert2Func[typeof(Core.torrent_added_alert)] = a => OnTorrentAddAlert((Core.torrent_added_alert)a);
            Alert2Func[typeof(Core.state_update_alert)] = a => OnTorrentUpdateAlert((Core.state_update_alert)a);
            //Alert2Func[typeof(Core.torrent_paused_alert)] = a => OnTorrentPausedAlert((Core.torrent_paused_alert)a);
            //Alert2Func[typeof(Core.torrent_resumed_alert)] = a => OnTorrentResumedAlert((Core.torrent_resumed_alert)a);
            //Alert2Func[typeof(Core.torrent_removed_alert)] = a => OnTorrentRemovedAlert((Core.torrent_removed_alert)a); // torrent removed from torrentSession, always ok
            //Alert2Func[typeof(Core.torrent_deleted_alert)] = a => OnTorrentDeletedAlert((Core.torrent_deleted_alert)a); // finished deleting file for removed torrent

            _dispatcherTimer.Elapsed += new ElapsedEventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = 100;//Settings.Application.DISPATCHER_INTERVAL;
            _dispatcherTimer.Start();
            
        }

        public static void Terminate()
        {
            _dispatcherTimer.Stop();
            _torrentSession.clear_alert_dispatch();
            //foreach (KeyValuePair<string, Core.TorrentHandle> item in TorrentHandles)
            //{
            //    item.Value.Dispose();
            //}
            _torrentSession.Dispose();
        }

        private static Core.TorrentHandle getTorrentHandle(string hash)
        {
            log.Trace("requested getTorrentHandle({0})", hash);
            Core.TorrentHandle th;
            if (TorrentHandles.TryGetValue(hash, out th))
            {
                return th;
            }
            else
            {
                string smgs = "cannot find requested torrent in Tsunami";
                log.Error(smgs);
                throw new Exception(smgs);
            }
        }

        public static Models.TorrentStatus getTorrentStatus(string hash)
        {
            Core.TorrentHandle th;
            log.Trace("requested getTorrentStatus({0})", hash);
            if (TorrentHandles.TryGetValue(hash, out th))
            {
                return new Models.TorrentStatus(th.status());
            }
            else
            {
                string smgs = "cannot find requested torrent in Tsunami";
                log.Error(smgs);
                throw new Exception(smgs);
            }
        }

        public static List<Models.TorrentStatus> getTorrentStatusList()
        {
            List<Models.TorrentStatus> thl = new List<Models.TorrentStatus>();
            foreach (KeyValuePair<string, Core.TorrentHandle> item in TorrentHandles)
            {
                Core.TorrentHandle th = item.Value;
                thl.Add(new Models.TorrentStatus(th.status()));// Models.TorrentHandle(item.Value));
            }
            return thl;
        }

        public static void addTorrent(string filename)
        {
            using (var atp = new Core.AddTorrentParams())
            using (var ti = new Core.TorrentInfo(filename))
            {
                atp.save_path = Settings.User.PathDownload;
                atp.ti = ti;
                atp.flags &= ~Core.ATPFlags.flag_auto_managed; // remove auto managed flag
                atp.flags &= ~Core.ATPFlags.flag_paused; // remove pause on added torrent
                TorrentSession.async_add_torrent(atp);
            }
        }

        public static void deleteTorrent(string hash, bool deleteFileToo = false)
        {
            log.Trace("requested delete({0}, deleteFile:{1})", hash, deleteFileToo);
            Core.TorrentHandle th = getTorrentHandle(hash);
            TorrentSession.remove_torrent(th, Convert.ToInt32(deleteFileToo));
            TorrentHandles.TryRemove(hash, out th);
        }

        public static bool pauseTorrent(string hash)
        {
            log.Trace("requested pause({0})", hash);
            Core.TorrentHandle th = getTorrentHandle(hash);
            th.pause();
            return (th.status().paused == true);
        }

        public static bool resumeTorrent(string hash)
        {
            log.Trace("requested resume({0})", hash);
            Core.TorrentHandle th = getTorrentHandle(hash);
            th.resume();
            return (th.status().paused == true);
        }

        private static void dispatcherTimer_Tick(object sender, ElapsedEventArgs e)
        {
            _torrentSession.post_torrent_updates();
            Core.SessionStatus ss = TorrentSession.status();
            var evnt = new OnSessionStatisticsEventArgs()
            {
                DownloadRate = ss.download_rate,
                UploadRate = ss.upload_rate
            };

            // notify web
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
            context.Clients.All.notifySessionStatistics(evnt);

            // invoke event
            SessionStatisticsUpdate?.Invoke(null, evnt);
        }

        public static string GiveMeStateFromEnum(System.Enum stateFromCore)
        {
            string res = "";
            switch (stateFromCore.ToString())
            {
                case "queued_for_checking":
                    res = "Queued For Checking";
                    break;
                case "checking_files":
                    res = "Checking Files";
                    break;
                case "downloading_metadata":
                    res = "Downloading Metadata";
                    break;
                case "downloading":
                    res = "Downloading";
                    break;
                case "finished":
                    res = "Finished";
                    break;
                case "seeding":
                    res = "Seeding";
                    break;
                case "allocating":
                    res = "Allocating";
                    break;
                case "checking_resume_data":
                    res = "Checking Resume Data";
                    break;
                default:
                    res = "Error";
                    break;
            }
            return res;
        }

        public static string GiveMeStorageModeFromEnum(System.Enum smFromCore)
        {
            string sRes = "";
            switch (smFromCore.ToString())
            {
                case "storage_mode_allocate":
                    sRes = "Allocate";
                    break;
                case "storage_mode_sparse":
                    sRes = "Sparse";
                    break;
                default:
                    sRes = "Sparse";
                    break;
            }
            return sRes;
        }
 

        private static void HandleAlertCallback(Core.Alert a)
        {
            using (a)
            {
                Action<Object> run;
                if (Alert2Func.TryGetValue(a.GetType(), out run))
                {
                    run(a);
                }
            }

            //using (a)
            //{
            //    // UPDATED
            //    if (a.GetType() == typeof(Core.state_update_alert))
            //    {
            //        OnTorrentUpdateAlert((Core.state_update_alert)a);
            //    }
            //    // ADDED
            //    else if (a.GetType() == typeof(Core.torrent_added_alert))
            //    {
            //        Core.torrent_added_alert taa = (Core.torrent_added_alert)a;
            //        log.Debug("torrent added: name {0}; status {1}; hash {2}", taa.handle.status().name, taa.handle.status().state.ToString(), taa.handle.status().info_hash.ToString());
            //        OnTorrentAddAlert((Core.torrent_added_alert)a);
            //    }
            //    // PAUSED
            //    else if (a.GetType() == typeof(Core.torrent_paused_alert)) {
            //        Core.torrent_paused_alert tpa = (Core.torrent_paused_alert)a;
            //        log.Debug("torrent " + tpa.handle.status().name + " paused");
            //    }
            //    // RESUMED
            //    else if (a.GetType() == typeof(Core.torrent_resumed_alert))
            //    {
            //        Core.torrent_resumed_alert tra = (Core.torrent_resumed_alert)a;
            //        log.Debug("torrent " + tra.handle.status().name + " resumed");
            //    }
            //    // REMOVED 
            //    else if (a.GetType() == typeof(Core.torrent_removed_alert))
            //    {
            //        Core.torrent_removed_alert tra = (Core.torrent_removed_alert)a;
            //        TorrentRemoved?.Invoke(null, new OnTorrentRemovedEventArgs() { Hash = tra.handle.info_hash().ToString() });
            //        log.Debug("torrent " + tra.handle.status().name + " removed");
            //    }
            //    // DELETED 
            //    else if (a.GetType() == typeof(Core.torrent_deleted_alert))
            //    {
            //        Core.torrent_deleted_alert tra = (Core.torrent_deleted_alert)a;
            //        log.Debug("files for torrent " + tra.handle.status().name + " deleted");
            //    }
            //}
        }

        private static void OnTorrentUpdateAlert(Core.state_update_alert a)
        {
            foreach (Core.TorrentStatus ts in a.status)
            {
                var stat = "Paused";
                if (!ts.paused)
                {
                    stat = GiveMeStateFromEnum(ts.state);
                }
                var evnt = new OnTorrentUpdatedEventArgs(ts);
                evnt.State = stat;
                //log.Trace("torrent: name {0}; status {1}; progress {2}", ts.name, ts.state.ToString(), ts.progress);

                var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
                context.Clients.All.notifyUpdateProgress(evnt);

                TorrentUpdated?.Invoke(null, evnt);
            }
        }

        private static void OnTorrentAddAlert(Core.torrent_added_alert a)
        {
            Core.TorrentHandle th = a.handle;
            if (TorrentHandles.TryAdd(th.info_hash().ToString(), th))
            {
                using (Core.TorrentStatus ts = th.status())
                {
                    var stat = "Paused";
                    if (!ts.paused)
                    {
                        stat = GiveMeStateFromEnum(ts.state);
                    }
                    var evnt = new OnTorrentAddedEventArgs
                    {
                        Hash = th.info_hash().ToString(),
                        Name = ts.name,
                        Progress = ts.progress,
                        QueuePosition = ts.queue_position,
                        Status = stat
                    };
                    //log.Debug("torrent added: name {0}; status {1}; hash {2}", ts.name, ts.state.ToString(), ts.info_hash.ToString());

                    // notify web that a new id must be requested via webapi
                    var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
                    context.Clients.All.notifyTorrentAdded(evnt.Hash);

                    TorrentAdded?.Invoke(null, evnt);
                }
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
            public int ActiveTime { get; set; }
            public DateTime AddedTime { get; set; }
            public long AllTimeDownload { get; set; }
            public long AllTimeUpload { get; set; }
            public TimeSpan AnnounceInterval { get; set; }
            public bool AutoManaged { get; set; }
            public int BlockSize { get; set; }
            public DateTime CompletedTime { get; set; }
            public int ConnectionsLimit { get; set; }
            public int ConnectCandidates { get; set; }
            public string CurrentTracker { get; set; }
            public float DistributedCopies { get; set; }
            public int DistributedFraction { get; set; }
            public int DistributedFullCopies { get; set; }
            public int DownloadPayloadRate { get; set; }
            public int DownloadRate { get; set; }
            public int DownBandwidthQueue { get; set; }
            public string Error { get; set; }
            public int FinishedTime { get; set; }
            public bool HasIncoming { get; set; }
            public bool HasMetadata { get; set; }
            public string InfoHash { get; set; }
            public bool IpFilterApplies { get; set; }
            public bool IsFinished { get; set; }
            public bool IsSeeding { get; set; }
            public int LastScrape { get; set; }
            public DateTime LastSeenComplete { get; set; }
            public int ListPeers { get; set; }
            public int ListSeeds { get; set; }
            public bool MovingStorage { get; set; }
            public string Name { get; set; }
            public bool NeedSaveResume { get; set; }
            public TimeSpan NextAnnounce { get; set; }
            public int NumComplete { get; set; }
            public int NumConnections { get; set; }
            public int NumIncomplete { get; set; }
            public int NumPeers { get; set; }
            public int NumPieces { get; set; }
            public int NumSeeds { get; set; }
            public int NumUploads { get; set; }
            public bool Paused { get; set; }
            public Models.BitField Pieces { get; set; }
            public int Priority { get; set; }
            public float Progress { get; set; }
            public int ProgressPpm { get; set; }
            public int QueuePosition { get; set; }
            public string SavePath { get; set; }
            public int SeedingTime { get; set; }
            public bool SeedMode { get; set; }
            public int SeedRank { get; set; }
            public bool SequentialDownload { get; set; }
            public bool ShareMode { get; set; }
            public string State { get; set; }
            public string StorageMode { get; set; }
            public bool SuperSeeding { get; set; }
            public int TimeSinceDownload { get; set; }
            public int TimeSinceUpload { get; set; }
            public long TotalDone { get; set; }
            public long TotalDownload { get; set; }
            public long TotalFailedBytes { get; set; }
            public long TotalPayloadDownload { get; set; }
            public long TotalPayloadUpload { get; set; }
            public long TotalReduntantBytes { get; set; }
            public long TotalUpload { get; set; }
            public long TotalWanted { get; set; }
            public long TotalWantedDone { get; set; }
            public int UploadsLimit { get; set; }
            public bool UploadMode { get; set; }
            public int UploadPayloadRate { get; set; }
            public int UploadRate { get; set; }
            public int UpBandwidthQueue { get; set; }
            public Models.BitField VerifiedPieces { get; set; }

            public OnTorrentUpdatedEventArgs() { /* nothing to do. just for serializator */ }

            public OnTorrentUpdatedEventArgs(Core.TorrentStatus ts)
            {
                ActiveTime = ts.active_time;
                AddedTime = ts.added_time;
                AllTimeDownload = ts.all_time_download;
                AllTimeUpload = ts.all_time_upload;
                AnnounceInterval = ts.announce_interval;
                AutoManaged = ts.auto_managed;
                BlockSize = ts.block_size;
                CompletedTime = ts.completed_time;
                ConnectionsLimit = ts.connections_limit;
                ConnectCandidates = ts.connect_candidates;
                CurrentTracker = ts.current_tracker;
                DistributedCopies = ts.distributed_copies;
                DistributedFraction = ts.distributed_fraction;
                DistributedFullCopies = ts.distributed_full_copies;
                DownloadPayloadRate = ts.download_payload_rate;
                DownloadRate = ts.download_rate;
                DownBandwidthQueue = ts.down_bandwidth_queue;
                Error = ts.error;
                FinishedTime = ts.finished_time;
                HasIncoming = ts.has_incoming;
                HasMetadata = ts.has_metadata;
                InfoHash = ts.info_hash.ToString();
                IpFilterApplies = ts.ip_filter_applies;
                IsFinished = ts.is_finished;
                IsSeeding = ts.is_seeding;
                LastScrape = ts.last_scrape;
                LastSeenComplete = ts.last_seen_complete;
                ListPeers = ts.list_peers;
                ListSeeds = ts.list_seeds;
                MovingStorage = ts.moving_storage;
                Name = ts.name;
                NeedSaveResume = ts.need_save_resume;
                NextAnnounce = ts.next_announce;
                NumComplete = ts.num_complete;
                NumConnections = ts.num_connections;
                NumIncomplete = ts.num_incomplete;
                NumPeers = ts.num_peers;
                NumPieces = ts.num_pieces;
                NumSeeds = ts.num_seeds;
                NumUploads = ts.num_uploads;
                Paused = ts.paused;
                Pieces = new Models.BitField(ts.pieces);
                Priority = ts.priority;
                Progress = ts.progress;
                ProgressPpm = ts.progress_ppm;
                QueuePosition = ts.queue_position;
                SavePath = ts.save_path;
                SeedingTime = ts.seeding_time;
                SeedMode = ts.seed_mode;
                SeedRank = ts.seed_rank;
                SequentialDownload = ts.sequential_download;
                ShareMode = ts.share_mode;
                State = GiveMeStateFromEnum(ts.state);
                StorageMode = GiveMeStorageModeFromEnum(ts.storage_mode);
                SuperSeeding = ts.super_seeding;
                TimeSinceDownload = ts.time_since_download;
                TimeSinceUpload = ts.time_since_upload;
                TotalDone = ts.total_done;
                TotalDownload = ts.total_download;
                TotalFailedBytes = ts.total_failed_bytes;
                TotalPayloadDownload = ts.total_payload_download;
                TotalPayloadUpload = ts.total_payload_upload;
                TotalReduntantBytes = ts.total_reduntant_bytes;
                TotalUpload = ts.total_upload;
                TotalWanted = ts.total_wanted;
                TotalWantedDone = ts.total_wanted_done;
                UploadsLimit = ts.uploads_limit;
                UploadMode = ts.upload_mode;
                UploadPayloadRate = ts.upload_payload_rate;
                UploadRate = ts.upload_rate;
                UpBandwidthQueue = ts.up_bandwidth_queue;
                VerifiedPieces = new Models.BitField(ts.verified_pieces);
            }

        }

        public class OnTorrentRemovedEventArgs : EventArgs
        {
            public string Hash { get; set; }
        }

        public class OnSessionStatisticsEventArgs
        {
            //public int allowed_upload_slots { get; set; }
            //public int dht_download_rate { get; set; }
            //public long dht_global_nodes { get; set; }
            //public int dht_nodes { get; set; }
            //public int dht_node_cache { get; set; }
            //public int dht_torrents { get; set; }
            //public int dht_total_allocations { get; set; }
            //public int dht_upload_rate { get; set; }
            //public int disk_read_queue { get; set; }
            //public int disk_write_queue { get; set; }
            public int DownloadRate { get; set; }
            //public int down_bandwidth_bytes_queue { get; set; }
            //public int down_bandwidth_queue { get; set; }
            //public bool has_incoming_connections { get; set; }
            //public int ip_overhead_download_rate { get; set; }
            //public int ip_overhead_upload_rate { get; set; }
            //public int num_peers { get; set; }
            //public int num_unchoked { get; set; }
            //public int optimistic_unchoke_counter { get; set; }
            //public int payload_download_rate { get; set; }
            //public int payload_upload_rate { get; set; }
            //public int peerlist_size { get; set; }
            //public long total_dht_download { get; set; }
            //public long total_dht_upload { get; set; }
            //public long total_download { get; set; }
            //public long total_failed_bytes { get; set; }
            //public long total_ip_overhead_download { get; set; }
            //public long total_ip_overhead_upload { get; set; }
            //public long total_payload_download { get; set; }
            //public long total_payload_upload { get; set; }
            //public long total_redundant_bytes { get; set; }
            //public long total_tracker_download { get; set; }
            //public long total_tracker_upload { get; set; }
            //public long total_upload { get; set; }
            //public int tracker_download_rate { get; set; }
            //public int tracker_upload_rate { get; set; }
            //public int unchoke_counter { get; set; }
            public int UploadRate { get; set; }
            //public int up_bandwidth_bytes_queue { get; set; }
            //public int up_bandwidth_queue { get; set; }

        }
    }
}
