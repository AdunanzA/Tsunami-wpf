using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Timers;
using NLog;
using Microsoft.Owin.Hosting;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;
using Tsunami.Core;
using System.Threading.Tasks;
//using Microsoft.Owin;

//[assembly: OwinStartup(typeof(Tsunami.www.Startup))]

namespace Tsunami
{
    public sealed class SessionManager
    {
        private static readonly SessionManager instance = new SessionManager();
        private static readonly object _tsunamiLock = new object();

        static SessionManager()
        {
        }

        private SessionManager()
        {
        }

        public static SessionManager Instance
        {
            get
            {
                return instance;
            }
        }

        private Logger log = LogManager.GetLogger("SessionManager");

        private Core.Session _torrentSession = new Core.Session();

        //save_resume_variables
        private int outstanding_resume_data = 0;
        private AutoResetEvent no_more_data = new AutoResetEvent(false);
        private bool no_more_resume = false;

        private System.Timers.Timer _dispatcherTimer = new System.Timers.Timer();
        private System.Timers.Timer _sessionStatusDispatcherTimer = new System.Timers.Timer();

        private IDisposable webServer;

        public EventHandler<EventsArgs.OnTorrentUpdatedEventArgs> TorrentUpdated;
        public EventHandler<EventsArgs.OnTorrentAddedEventArgs> TorrentAdded;
        public EventHandler<EventsArgs.OnTorrentRemovedEventArgs> TorrentRemoved;
        public EventHandler<EventsArgs.OnSessionStatisticsEventArgs> SessionStatisticsUpdate;
        public EventHandler<Models.ErrorCode> TorrentError;

        private ConcurrentDictionary<string, Core.TorrentHandle> TorrentHandles = new ConcurrentDictionary<string, Core.TorrentHandle>();
        private Dictionary<Type, Action<Object>> Alert2Func = new Dictionary<Type, Action<Object>>();

        private Dictionary<string, StreamTorrent> _streamingList = new Dictionary<string, Tsunami.StreamTorrent>();
        public EventHandler<string> BufferingCompleted;
        private bool _isStreaming = false;
        private string _streamingHash;

        public void Initialize()
        {
            Settings.Logger.Inizialize();
            Settings.User.readFromFile();

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
                    lock (_tsunamiLock)
                    {
                        _torrentSession.load_state(entry);
                    }
                }
            }

            // http://www.libtorrent.org/reference-Alerts.html
            Alert2Func[typeof(Core.torrent_added_alert)] = a => OnTorrentAddAlert((Core.torrent_added_alert)a);
            Alert2Func[typeof(Core.state_update_alert)] = a => OnTorrentUpdateAlert((Core.state_update_alert)a);
            //Alert2Func[typeof(Core.torrent_paused_alert)] = a => OnTorrentPausedAlert((Core.torrent_paused_alert)a);
            //Alert2Func[typeof(Core.torrent_resumed_alert)] = a => OnTorrentResumedAlert((Core.torrent_resumed_alert)a);
            //Alert2Func[typeof(Core.torrent_removed_alert)] = a => OnTorrentRemovedAlert((Core.torrent_removed_alert)a); // torrent removed from torrentSession, always ok
            //Alert2Func[typeof(Core.torrent_deleted_alert)] = a => OnTorrentDeletedAlert((Core.torrent_deleted_alert)a); // finished deleting file for removed torrent
            Alert2Func[typeof(Core.torrent_error_alert)] = a => OnTorrentErrorAlert((Core.torrent_error_alert)a);
            //Alert2Func[typeof(Core.stats_alert)] = a => OnStatsAlert((Core.stats_alert)a);
            Alert2Func[typeof(Core.dht_stats_alert)] = a => OnDhtStatsAlert((Core.dht_stats_alert)a);

            Alert2Func[typeof(Core.save_resume_data_alert)] = a => OnSaveResumeDataAlert((Core.save_resume_data_alert)a);
            Alert2Func[typeof(Core.save_resume_data_failed_alert)] = a => OnSaveResumeDataFailedAlert((Core.save_resume_data_failed_alert)a);

            Alert2Func[typeof(Core.piece_finished_alert)] = a => OnPieceFinishedAlert((Core.piece_finished_alert)a);

            lock (_tsunamiLock)
            {
                _torrentSession.start_dht();
                //_torrentSession.start_lsd();
                _torrentSession.start_natpmp();
                _torrentSession.start_upnp();

                var alertMask = Core.AlertMask.error_notification
                                | Core.AlertMask.peer_notification
                                | Core.AlertMask.port_mapping_notification
                                | Core.AlertMask.storage_notification
                                | Core.AlertMask.tracker_notification
                                | Core.AlertMask.status_notification
                                | Core.AlertMask.ip_block_notification
                                | Core.AlertMask.progress_notification
                                | Core.AlertMask.stats_notification
                                ;

                _torrentSession.set_alert_mask(alertMask);
                _torrentSession.set_alert_dispatch(HandleAlertCallback);
            }



            _dispatcherTimer.Elapsed += new ElapsedEventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = Settings.Application.DISPATCHER_INTERVAL;
            _dispatcherTimer.Start();

            _sessionStatusDispatcherTimer.Elapsed += new ElapsedEventHandler(sessionStatusDispatcher_Tick);
            _sessionStatusDispatcherTimer.Interval = 1000;
            _sessionStatusDispatcherTimer.Start();

        }

        private void OnPieceFinishedAlert(piece_finished_alert a)
        {
            string hash = a.handle.info_hash().ToString();
            if (_streamingList.ContainsKey(hash) && _streamingList[hash].OnStreaming)
            {
                _streamingList[hash].ContinueOne();
            }
        }

        private void OnDhtStatsAlert(Core.dht_stats_alert a)
        {
            //var aaa = a.active_requests;
        }

        private void OnSaveResumeDataAlert(Core.save_resume_data_alert a)
        {
            var h = a.handle;
            string newfilePath = ("./Fastresume/" + h.info_hash().ToString() + ".fastresume");
            var data = Core.Util.bencode(a.resume_data);
            using (var bw = new BinaryWriter(new FileStream(newfilePath, FileMode.OpenOrCreate)))
            {
                bw.Write(data);
                bw.Close();
            }
            Interlocked.Decrement(ref outstanding_resume_data);
            if (outstanding_resume_data == 0 && no_more_resume)
                no_more_data.Set();
        }

        private void OnSaveResumeDataFailedAlert(Core.save_resume_data_failed_alert a)
        {
            Interlocked.Decrement(ref outstanding_resume_data);
            if (outstanding_resume_data == 0 && no_more_resume)
                no_more_data.Set();
        }

        public void LoadFastResumeData()
        {
            if (Directory.Exists("Fastresume"))
            {
                lock (_tsunamiLock)
                {
                    string[] files = Directory.GetFiles("Fastresume", "*.fastresume");
                    foreach (string s in files)
                    {
                        var data = File.ReadAllBytes(s);
                        var info_hash = Path.GetFileNameWithoutExtension(s);
                        var filename = "Fastresume/" + info_hash + ".torrent";
                        Core.TorrentInfo ti;
                        if (File.Exists(filename))
                            ti = new Core.TorrentInfo(filename);
                        else
                            ti = new Core.TorrentInfo(new Core.Sha1Hash(info_hash));
                        using (var atp = new Core.AddTorrentParams())
                        using (ti)
                        {
                            atp.ti = ti;
                            atp.save_path = Settings.User.PathDownload;
                            atp.resume_data = (sbyte[])(Array)data;
                            atp.flags &= ~Core.ATPFlags.flag_auto_managed; // remove auto managed flag
                            atp.flags &= ~Core.ATPFlags.flag_paused; // remove pause on added torrent
                            _torrentSession.async_add_torrent(atp);
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory("Fastresume");
            }
        }

        public void startWeb()
        {
            Uri baseAddress = new Uri(string.Format("http://{0}:{1}", Settings.User.WebAddress, Settings.User.WebPort));
            webServer = WebApp.Start<www.Startup>(baseAddress.ToString());
        }

        public void stopWeb()
        {
            webServer?.Dispose();
            webServer = null;
        }

        public void Terminate()
        {
            lock (_tsunamiLock)
            {
                _sessionStatusDispatcherTimer.Stop();
                _sessionStatusDispatcherTimer.Enabled = false;
                _dispatcherTimer.Stop();
                _dispatcherTimer.Enabled = false;
                stopWeb();

                _torrentSession.pause();

                /* http://libtorrent.org/reference-Core.html#save_resume_data() */
                foreach (var item in TorrentHandles)
                {
                    if (item.Value.is_valid())
                    {
                        Core.TorrentStatus ts = item.Value.status();
                        if (ts.has_metadata && ts.need_save_resume)
                        {
                            /* http://libtorrent.org/reference-Core.html#save_resume_flags_t */
                            item.Value.save_resume_data(1 | 2 | 4);
                            ++outstanding_resume_data;
                        }
                    }
                }
                no_more_resume = true;
                if (outstanding_resume_data != 0)
                    no_more_data.WaitOne();
            }
            TerminateSaveResume();
        }

        private void TerminateSaveResume()
        {
            _torrentSession.clear_alert_dispatch();
            using (var entry = _torrentSession.save_state(0xfffffff))
            {
                var data = Core.Util.bencode(entry);
                File.WriteAllBytes(".session_state", data);
            }
            Task.Run(() =>
            {
                _torrentSession?.Dispose();
                _torrentSession = null;
            });
        }

        public Core.TorrentHandle getTorrentHandle(string hash)
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

        public Models.TorrentStatus getTorrentStatus(string hash)
        {
            lock (_tsunamiLock)
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
        }

        public List<Models.FileEntry> getTorrentFiles(string hash)
        {
            lock (_tsunamiLock)
            {
                List<Models.FileEntry> feList = new List<Models.FileEntry>();
                Models.FileEntry fe;

                Core.TorrentHandle th = getTorrentHandle(hash);
                Core.TorrentInfo ti = th.torrent_file();
                for (int i = 0; i <= ti.num_files() - 1; i++)
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
        }

        public List<Models.TorrentStatus> getTorrentStatusList()
        {
            lock (_tsunamiLock)
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
        }

        public void addTorrent(string filename)
        {
            lock (_tsunamiLock)
            {
                var data = File.ReadAllBytes(filename);
                string newfilePath;
                using (var atp = new Core.AddTorrentParams())
                using (var ti = new Core.TorrentInfo(filename))
                {
                    atp.save_path = Settings.User.PathDownload;
                    atp.ti = ti;
                    atp.flags &= ~Core.ATPFlags.flag_auto_managed; // remove auto managed flag
                    atp.flags &= ~Core.ATPFlags.flag_paused; // remove pause on added torrent
                    atp.flags &= ~Core.ATPFlags.flag_use_resume_save_path; // 
                    newfilePath = "./Fastresume/" + ti.info_hash().ToString() + ".torrent";
                    if (!File.Exists(newfilePath))
                    {
                        using (var bw = new BinaryWriter(new FileStream(newfilePath, FileMode.Create)))
                        {
                            bw.Write(data);
                            bw.Close();
                        }
                    }
                    _torrentSession.async_add_torrent(atp);
                }
            }
        }

        public void addTorrent(byte[] buffer)
        {
            lock (_tsunamiLock)
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
        }

        public void deleteTorrent(string hash, bool deleteFileToo = false)
        {
            lock (_tsunamiLock)
            {
                log.Trace("requested delete({0}, deleteFile:{1})", hash, deleteFileToo);
                Core.TorrentHandle th = getTorrentHandle(hash);
                _torrentSession.remove_torrent(th, Convert.ToInt32(deleteFileToo));
                TorrentHandles.TryRemove(hash, out th);
                try
                {
                    File.Delete(Environment.CurrentDirectory + "./Fastresume/" + hash + ".fastresume");
                    File.Delete(Environment.CurrentDirectory + "./Fastresume/" + hash + ".torrent");
                }
                catch (Exception) { }
            }
        }

        public bool pauseTorrent(string hash)
        {
            lock (_tsunamiLock)
            {
                log.Trace("requested pause({0})", hash);
                Core.TorrentHandle th = getTorrentHandle(hash);
                th.pause();
                return (th.status().paused == true);
            }
        }

        public bool resumeTorrent(string hash)
        {
            lock (_tsunamiLock)
            {
                log.Trace("requested resume({0})", hash);
                Core.TorrentHandle th = getTorrentHandle(hash);
                th.resume();
                return (th.status().paused == true);
            }
        }

        private void dispatcherTimer_Tick(object sender, ElapsedEventArgs e)
        {
                _torrentSession.post_torrent_updates();
                _torrentSession.post_dht_stats();
                //_torrentSession.post_session_stats();
        }

        private void sessionStatusDispatcher_Tick(object sender, ElapsedEventArgs e)
        {
             var evnt = new EventsArgs.OnSessionStatisticsEventArgs(_torrentSession.status());

                // notify web
                if (webServer != null)
                {
                    var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
                    context.Clients.All.notifySessionStatistics(evnt);
                }

                // invoke event
                SessionStatisticsUpdate?.Invoke(null, evnt);
        }

        private void HandleAlertCallback(Core.Alert a)
        {
            using (a)
            {
                Action<object> run;
                if (Alert2Func.TryGetValue(a.GetType(), out run))
                {
                    run(a);
                }
            }
        }

        private void OnTorrentUpdateAlert(Core.state_update_alert a)
        {
            lock (_tsunamiLock)
            {
                foreach (Core.TorrentStatus ts in a.status)
                {
                    var stat = "Paused";
                    if (!ts.paused)
                    {
                        stat = Utils.GiveMeStateFromEnum(ts.state);
                    }
                    var evnt = new EventsArgs.OnTorrentUpdatedEventArgs(ts);
                    evnt.State = stat;
                    //log.Trace("torrent: name {0}; status {1}; progress {2}", ts.name, ts.state.ToString(), ts.progress);

                    if (webServer != null)
                    {
                        var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
                        context.Clients.All.notifyUpdateProgress(evnt);
                    }

                    TorrentUpdated?.Invoke(null, evnt);
                }
            }
        }

        private void OnTorrentAddAlert(Core.torrent_added_alert a)
        {
            lock (_tsunamiLock)
            {
                Core.TorrentHandle th = a.handle;
                if (TorrentHandles.TryAdd(th.info_hash().ToString(), th))
                {
                    using (Core.TorrentStatus ts = th.status())
                    {
                        var stat = "Paused";
                        if (!ts.paused)
                        {
                            stat = Utils.GiveMeStateFromEnum(ts.state);
                        }
                        var evnt = new EventsArgs.OnTorrentAddedEventArgs
                        {
                            Hash = th.info_hash().ToString(),
                            Name = ts.name,
                            Progress = ts.progress,
                            QueuePosition = ts.queue_position,
                            Status = stat
                        };
                        //log.Debug("torrent added: name {0}; status {1}; hash {2}", ts.name, ts.state.ToString(), ts.info_hash.ToString());

                        // notify web that a new id must be requested via webapi
                        if (webServer != null)
                        {
                            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
                            context.Clients.All.notifyTorrentAdded(evnt.Hash);
                        }

                        TorrentAdded?.Invoke(null, evnt);
                    }
                }
            }
        }

        private void OnTorrentErrorAlert(Core.torrent_error_alert a)
        {
            var evnt = new Models.ErrorCode(a.error);

            //notify web
            if (webServer != null)
            {
                var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<www.SignalRHub>();
                context.Clients.All.notifyTorrentAdded(evnt);
            }

            TorrentError?.Invoke(null, evnt);
        }

        private void OnStatsAlert(Core.stats_alert a)
        {

            int upload_payload = a.transferred[0];
            int upload_protocol = a.transferred[1];
            int download_payload = a.transferred[2];
            int download_protocol = a.transferred[3];
            int upload_ip_protocol = a.transferred[4];
            //int deprecated1 = a.transferred[5];
            //int deprecated2 = a.transferred[6];
            int download_ip_protocol = a.transferred[7];
            //int deprecated3 = a.transferred[8];
            //int deprecated4 = a.transferred[9];
            int num_channels = a.transferred[10];
        }

        public string GetFilePathFromHash(string hash, int fileIndex)
        {
            lock (_tsunamiLock)
            {
                Core.TorrentHandle th = getTorrentHandle(hash);
                var ti = th.torrent_file();
                var files = ti.files();
                var fileEntry = files.at(fileIndex);
                string path = Settings.User.PathDownload + "\\" + fileEntry.path;
                return path;
            }
        }

        public void StreamTorrent(string hash, int fileIndex)
        {
            if (_streamingList.ContainsKey(hash))
            {
                _streamingList[hash].ContinueStreaming(BufferingReadyCallback);
            }
            else
            {
                _streamingList[hash] = new StreamTorrent(hash, fileIndex, BufferingReadyCallback);
                _isStreaming = true;
                _streamingHash = hash;
            }
        }

        private void BufferingReadyCallback(object sender, string path)
        {
            BufferingCompleted?.Invoke(sender, path);
        }

        public bool IsStreaming()
        {
            return _isStreaming;
        }

        public void StopStreaming()
        {
            if (_streamingList.ContainsKey(_streamingHash))
            {
                BufferingCompleted = null;
                _streamingList[_streamingHash].StopStreaming(BufferingReadyCallback);
                _isStreaming = false;
            }
        }
    }
}
