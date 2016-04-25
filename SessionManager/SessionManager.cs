using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NLog;
using Microsoft.Owin.Hosting;
using System.IO;
//using Tsunami.Core;
//using Microsoft.Owin;

//[assembly: OwinStartup(typeof(Tsunami.www.Startup))]

namespace Tsunami
{
    public static class SessionManager
    {
        private static Logger log = LogManager.GetLogger("SessionManager");
        private static Core.Session _torrentSession = new Core.Session();

        private static Timer _dispatcherTimer = new Timer();
        private static Timer _sessionStatusDispatcherTimer = new Timer();

        private static IDisposable webServer;

        public static EventHandler<OnTorrentUpdatedEventArgs> TorrentUpdated;
        public static EventHandler<OnTorrentAddedEventArgs> TorrentAdded;
        public static EventHandler<OnTorrentRemovedEventArgs> TorrentRemoved;
        public static EventHandler<OnSessionStatisticsEventArgs> SessionStatisticsUpdate;
        public static EventHandler<Models.ErrorCode> TorrentError;

        private static ConcurrentDictionary<string, Core.TorrentHandle> TorrentHandles = new ConcurrentDictionary<string, Core.TorrentHandle>();
        private static Dictionary<Type, Action<Object>> Alert2Func = new Dictionary<Type, Action<Object>>();

        //private static Core.Session TorrentSession { get; set; }

        public static void Initialize()
        {
            Settings.Logger.Inizialize();

            // web server
            if (Settings.User.StartWebOnAppLoad)
            {
                startWeb();
            }

            if (File.Exists(".session_state"))
            {
                var data = File.ReadAllBytes(".session_state");
                using (var entry = Core.Util.lazy_bdecode(data))
                {
                    _torrentSession.load_state(entry);
                }
            }

            _torrentSession.start_dht();
            _torrentSession.start_lsd();
            _torrentSession.start_natpmp();
            _torrentSession.start_upnp();

            Alert2Func[typeof(Core.torrent_added_alert)] = a => OnTorrentAddAlert((Core.torrent_added_alert)a);
            Alert2Func[typeof(Core.state_update_alert)] = a => OnTorrentUpdateAlert((Core.state_update_alert)a);
            //Alert2Func[typeof(Core.torrent_paused_alert)] = a => OnTorrentPausedAlert((Core.torrent_paused_alert)a);
            //Alert2Func[typeof(Core.torrent_resumed_alert)] = a => OnTorrentResumedAlert((Core.torrent_resumed_alert)a);
            //Alert2Func[typeof(Core.torrent_removed_alert)] = a => OnTorrentRemovedAlert((Core.torrent_removed_alert)a); // torrent removed from torrentSession, always ok
            //Alert2Func[typeof(Core.torrent_deleted_alert)] = a => OnTorrentDeletedAlert((Core.torrent_deleted_alert)a); // finished deleting file for removed torrent
            Alert2Func[typeof(Core.torrent_error_alert)] = a => OnTorrentErrorAlert((Core.torrent_error_alert)a);

            _dispatcherTimer.Elapsed += new ElapsedEventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = Settings.Application.DISPATCHER_INTERVAL;
            _dispatcherTimer.Start();

            _sessionStatusDispatcherTimer.Elapsed += new ElapsedEventHandler(sessionStatusDispatcher_Tick);
            _sessionStatusDispatcherTimer.Interval = 1000;
            _sessionStatusDispatcherTimer.Start();

            // http://www.libtorrent.org/reference-Alerts.html
            _torrentSession.set_alert_mask(Core.AlertMask.all_categories);
            _torrentSession.set_alert_dispatch(HandleAlertCallback);

        }

        public static void startWeb()
        {
            Uri baseAddress = new Uri(string.Format("http://{0}:{1}", Settings.User.WebAddress, Settings.User.WebPort));
            webServer = WebApp.Start<www.Startup>(baseAddress.ToString());
        }

        public static void stopWeb()
        {
            webServer?.Dispose();
            webServer = null;
        }

        public static void Terminate()
        {
            _dispatcherTimer.Stop();
            _sessionStatusDispatcherTimer.Stop();
            _torrentSession.clear_alert_dispatch();
            _torrentSession.pause();

            /* http://libtorrent.org/reference-Core.html#save_resume_data() */
            foreach (Core.TorrentHandle item in _torrentSession.get_torrents())
            {
                if (item.is_valid())
                {
                    Core.TorrentStatus ts = item.status();
                    if (ts.has_metadata && ts.need_save_resume)
                    {
                        /* http://libtorrent.org/reference-Core.html#save_resume_flags_t */
                        item.save_resume_data(1 | 2 | 4);
                    }
                }
            }
            using (var entry = _torrentSession.save_state(0xfffffff))
            {
                var data = Core.Util.bencode(entry);
                File.WriteAllBytes(".session_state", data);
            }
            _torrentSession.Dispose();
            stopWeb();
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

        public static List<Models.FileEntry> getTorrentFiles(string hash)
        {
            List<Models.FileEntry> feList = new List<Models.FileEntry>();
            Models.FileEntry fe;

            Core.TorrentHandle th = getTorrentHandle(hash);
            Core.TorrentInfo ti = th.torrent_file();
            for (int i = 0; i <= ti.num_files()-1; i++)
            {
                fe = new Models.FileEntry(ti.files().at(i));
                fe.FileName = ti.files().file_name(i);
                fe.IsValid = ti.files().is_valid();
                fe.PieceSize = ti.piece_size(i);
                //ti.files().name(); ???
                //ti.trackers();
                feList.Add(fe);
            }
            return feList;
        }

        public static List<Models.TorrentStatus> getTorrentStatusList()
        {
            _dispatcherTimer.Stop();
            _sessionStatusDispatcherTimer.Stop();
            List<Models.TorrentStatus> thl = new List<Models.TorrentStatus>();
            foreach (KeyValuePair<string, Core.TorrentHandle> item in TorrentHandles)
            {
                Core.TorrentHandle th = item.Value;
                thl.Add(new Models.TorrentStatus(th.status()));// Models.TorrentHandle(item.Value));
            }
            _dispatcherTimer.Start();
            _sessionStatusDispatcherTimer.Start();
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
                atp.flags &= ~Core.ATPFlags.flag_use_resume_save_path; // 
                _torrentSession.async_add_torrent(atp);
            }
        }

        public static void addTorrent(byte[] buffer)
        {
            using (var atp = new Core.AddTorrentParams())
            using (var ti = new Core.TorrentInfo(buffer))
            {
                atp.save_path = Settings.User.PathDownload;
                atp.ti = ti;
                atp.flags &= ~Core.ATPFlags.flag_auto_managed; // remove auto managed flag
                atp.flags &= ~Core.ATPFlags.flag_paused; // remove pause on added torrent
                _torrentSession.async_add_torrent(atp);
            }
        }

        public static void deleteTorrent(string hash, bool deleteFileToo = false)
        {
            log.Trace("requested delete({0}, deleteFile:{1})", hash, deleteFileToo);
            Core.TorrentHandle th = getTorrentHandle(hash);
            _torrentSession.remove_torrent(th, Convert.ToInt32(deleteFileToo));
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
        }

        private static void sessionStatusDispatcher_Tick(object sender, ElapsedEventArgs e)
        {
            var evnt = new OnSessionStatisticsEventArgs(_torrentSession.status());
            
            // notify web
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
            context.Clients.All.notifySessionStatistics(evnt);

            // invoke event
            SessionStatisticsUpdate?.Invoke(null, evnt);
        }



        public static string GiveMeStateFromEnum(Enum stateFromCore)
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

        public static string GiveMeStorageModeFromEnum(Enum smFromCore)
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

        private static void OnTorrentErrorAlert(Core.torrent_error_alert a)
        {
            var evnt = new Models.ErrorCode(a.error);

            //notify web
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
            context.Clients.All.notifyTorrentAdded(evnt);

            TorrentError?.Invoke(null, evnt);
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

        public class OnSessionStatisticsEventArgs : EventArgs
        {
            public int AllowedUploadSlots { get; set; }
            public int DhtDownloadRate { get; set; }
            public long DhtGlobalNodes { get; set; }
            public int DhtNodes { get; set; }
            public int DhtNodeCache { get; set; }
            public int DhtTorrents { get; set; }
            public int DhtTotalAllocations { get; set; }
            public int DhtUploadRate { get; set; }
            public int DiskReadQueue { get; set; }
            public int DiskWriteQueue { get; set; }
            public int DownloadRate { get; set; }
            public int DownBandwidthBytesQueue { get; set; }
            public int DownBandwidthQueue { get; set; }
            public bool HasIncomingConnections { get; set; }
            public int IpOverheadDownloadRate { get; set; }
            public int IpOverheadUploadRate { get; set; }
            public int NumPeers { get; set; }
            public int NumUnchoked { get; set; }
            public int OptimisticUnchokeCounter { get; set; }
            public int PayloadDownloadRate { get; set; }
            public int PayloadUploadRate { get; set; }
            public int PeerlistSize { get; set; }
            public long TotalDhtDownload { get; set; }
            public long TotalDhtUpload { get; set; }
            public long TotalDownload { get; set; }
            public long TotalFailedBytes { get; set; }
            public long TotalIpOverheadDownload { get; set; }
            public long TotalIpOverheadUpload { get; set; }
            public long TotalPayloadDownload { get; set; }
            public long TotalPayloadUpload { get; set; }
            public long TotalRedundantBytes { get; set; }
            public long TotalTrackerDownload { get; set; }
            public long TotalTrackerUpload { get; set; }
            public long TotalUpload { get; set; }
            public int TrackerDownloadRate { get; set; }
            public int TrackerUploadRate { get; set; }
            public int UnchokeCounter { get; set; }
            public int UploadRate { get; set; }
            public int UpBandwidthBytesQueue { get; set; }
            public int UpBandwidthQueue { get; set; }

            public OnSessionStatisticsEventArgs() { /* nothing to do. just for serializator */ }

            public OnSessionStatisticsEventArgs(Core.SessionStatus ss)
            {
                AllowedUploadSlots = ss.allowed_upload_slots;
                DhtDownloadRate = ss.dht_download_rate;
                DhtGlobalNodes = ss.dht_global_nodes;
                DhtNodes = ss.dht_nodes;
                DhtNodeCache = ss.dht_node_cache;
                DhtTorrents = ss.dht_torrents;
                DhtTotalAllocations = ss.dht_total_allocations;
                DhtUploadRate = ss.dht_upload_rate;
                DiskReadQueue = ss.disk_read_queue;
                DiskWriteQueue = ss.disk_write_queue;
                DownloadRate = ss.download_rate;
                DownBandwidthBytesQueue = ss.down_bandwidth_bytes_queue;
                DownBandwidthQueue = ss.down_bandwidth_queue;
                HasIncomingConnections = ss.has_incoming_connections;
                IpOverheadDownloadRate = ss.ip_overhead_download_rate;
                IpOverheadUploadRate = ss.ip_overhead_upload_rate;
                NumPeers = ss.num_peers;
                NumUnchoked = ss.num_unchoked;
                OptimisticUnchokeCounter = ss.optimistic_unchoke_counter;
                PayloadDownloadRate = ss.payload_download_rate;
                PayloadUploadRate = ss.payload_upload_rate;
                PeerlistSize = ss.peerlist_size;
                TotalDhtDownload = ss.total_dht_download;
                TotalDhtUpload = ss.total_dht_upload;
                TotalDownload = ss.total_download;
                TotalFailedBytes = ss.total_failed_bytes;
                TotalIpOverheadDownload = ss.total_ip_overhead_download;
                TotalIpOverheadUpload = ss.total_ip_overhead_upload;
                TotalPayloadDownload = ss.total_payload_download;
                TotalPayloadUpload = ss.total_payload_upload;
                TotalRedundantBytes = ss.total_redundant_bytes;
                TotalTrackerDownload = ss.total_tracker_download;
                TotalTrackerUpload = ss.total_tracker_upload;
                TotalUpload = ss.total_upload;
                TrackerDownloadRate = ss.tracker_download_rate;
                TrackerUploadRate = ss.tracker_upload_rate;
                UnchokeCounter = ss.unchoke_counter;
                UploadRate = ss.upload_rate;
                UpBandwidthBytesQueue = ss.up_bandwidth_bytes_queue;
                UpBandwidthQueue = ss.up_bandwidth_queue;
            }
        }
    }
}
