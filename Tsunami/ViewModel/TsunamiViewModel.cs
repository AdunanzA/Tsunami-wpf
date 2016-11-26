﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NLog;
using System.Windows.Input;
using System.Windows;
using Tsunami.Core;
using System.Threading;
using System.IO;
using System.ComponentModel;

namespace Tsunami.ViewModel
{
    class TsunamiViewModel : INotifyPropertyChanged, IDisposable
    {
        private static object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;
        private void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private readonly Logger log = LogManager.GetLogger("TsunamiViewModel");
        private readonly Logger CoreLog = LogManager.GetLogger("Core");

        //save_resume_variables
        private int outstanding_resume_data = 0;
        private readonly AutoResetEvent no_more_data = new AutoResetEvent(false);
        private bool no_more_resume = false;
        private DateTime _lastSaveResumeExecution = DateTime.Now;

        private readonly System.Timers.Timer _dispatcherTimer = new System.Timers.Timer();

        private static ObservableCollection<Models.TorrentItem> _torrentList { get; set; }
        public static ObservableCollection<Models.TorrentItem> TorrentList
        {
            get
            {
                return _torrentList;
            }
        }

        private SessionStatistics _sessionStatistic { get; set; }
        public SessionStatistics SessionStatistic
        {
            get
            {
                return _sessionStatistic;
            }
        }

        private Models.Preferences _preference { get; set; }
        public Models.Preferences Preference
        {
            get
            {
                return _preference;
            }
        }

        private bool _isTsunamiEnabled { get; set; }
        public bool IsTsunamiEnabled
        {
            get { return _isTsunamiEnabled; }
            set { if (_isTsunamiEnabled != value) { _isTsunamiEnabled = value; CallPropertyChanged("IsTsunamiEnabled"); } }
        }

        private double fileToLoad { get; set; }
        private double fileLoading { get; set; }
        private string stringLoading { get; set; }

        public double FileToLoad { get { return fileToLoad; } set { fileToLoad = value; CallPropertyChanged("FileToLoad"); } }
        public double FileLoading { get { return fileLoading; } set { fileLoading = value; CallPropertyChanged("FileLoading"); } }
        public string StringLoading { get { return stringLoading; } set { stringLoading = value; CallPropertyChanged("StringLoading"); } }

        private static Core.Session _torrentSession;

        public static Core.AdunanzaDht Dht;
        
        public ICommand AddClick
        {
            get
            {
                return _addClick ?? (_addClick = new CommandHandler(() => AddClick_Dialog(), _addClickCanExecute));
            }
        }
        private ICommand _addClick;
        private readonly bool _addClickCanExecute = true;
        readonly Dictionary<Type, int> alertType = new Dictionary<Type, int>
        {
            {typeof(Core.torrent_added_alert),0},
            {typeof(Core.state_update_alert),1},
            {typeof(Core.torrent_paused_alert),2},
            {typeof(Core.torrent_resumed_alert),3},
            {typeof(Core.torrent_removed_alert),4},
            {typeof(Core.torrent_deleted_alert),5},
            {typeof(Core.torrent_error_alert),6},
            {typeof(Core.dht_stats_alert),7},
            {typeof(Core.dht_bootstrap_alert),8},
            {typeof(Core.save_resume_data_alert),9},
            {typeof(Core.save_resume_data_failed_alert),10},
            {typeof(Core.piece_finished_alert),11}
        };

        public TsunamiViewModel()
        {
            Settings.Logger.Inizialize();

            _torrentList = new ObservableCollection<Models.TorrentItem>();
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(TorrentList, _lock);

            _sessionStatistic = new SessionStatistics();
            _preference = new Models.Preferences();
            IsTsunamiEnabled = false;
            //_notifyIcon = new Models.NotifyIcon();

            _torrentSession = new Core.Session();
            _torrentSession.pause();

            if (System.IO.File.Exists(".session_state"))
            {
                var data = System.IO.File.ReadAllBytes(".session_state");
                using (var entry = Core.Util.lazy_bdecode(data))
                {
                    _torrentSession.load_state(entry);
                }
            }

            Core.SessionSettings ss = _torrentSession.settings();
            //ss.connections_limit = 400; // 200
            //ss.tick_interval = 500;     // 500
            //ss.torrent_connect_boost = 20; // 10
            //ss.connection_speed = -1; // -1 = 200 ; default 10
            //ss.num_want = 400; // 200
            //ss.cache_size = -1; // -1 = 1/8 RAM; default 1024
            //ss.coalesce_reads = true; // false
            //ss.coalesce_writes = true; // false
            ss.user_agent = Tsunami.Settings.Application.TSUNAMI_USER_AGENT;
            _torrentSession.set_settings(ss);
            
            Core.DhtSettings dhts = _torrentSession.get_dht_settings();
            //dhts.aggressive_lookups = true;
            _torrentSession.set_dht_settings(dhts);

            var alertMask = Core.AlertMask.error_notification
                            | Core.AlertMask.peer_notification
                            | Core.AlertMask.port_mapping_notification
                            | Core.AlertMask.storage_notification
                            | Core.AlertMask.tracker_notification
                            | Core.AlertMask.status_notification
                            | Core.AlertMask.ip_block_notification
                            | Core.AlertMask.progress_notification
                            | Core.AlertMask.stats_notification
                            | Core.AlertMask.dht_notification
                            ;

            _torrentSession.set_alert_mask(alertMask);
            _torrentSession.set_alert_callback(HandlePendingAlertCallback);
            _torrentSession.set_session_callback(HandleAlertCallback);

            _dispatcherTimer.Elapsed += new System.Timers.ElapsedEventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = Settings.Application.DISPATCHER_INTERVAL;
            _dispatcherTimer.Start();

            LoadFastResumeData();

            _torrentSession.start_natpmp();
            _torrentSession.start_upnp();
            _torrentSession.start_dht();
            _torrentSession.resume();
            
            log.Debug("created");

            //Dht = new Core.AdunanzaDht();
            //Dht.start();
            //Dht.bootstrap("bootstrap.ring.cx", "4222");

        }

        private void dispatcherTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //if (!IsTsunamiEnabled) { return; }

            if (_torrentSession.alerts_empty())
            {
                _torrentSession.post_torrent_updates();
                _torrentSession.post_dht_stats();
                using (Core.SessionStatus ss = _torrentSession.status())
                {
                    SessionStatistic.Update(ss);
                }
            }


            //TimeSpan difference = DateTime.Now - _lastSaveResumeExecution;
            //if ( difference.TotalMinutes >= Settings.Application.SAVE_RESUME_INTERVAL && IsTsunamiEnabled)
            //{
            //    //int totConnex = 0;
            //    _lastSaveResumeExecution = DateTime.Now;

            //    List<Models.TorrentItem> myList = new List<Models.TorrentItem>(TorrentList);
                
            //    foreach (Models.TorrentItem item in myList)
            //    {
            //        using (Core.Sha1Hash sha1hash = new Core.Sha1Hash(item.Hash))
            //        using (Core.TorrentHandle th = _torrentSession.find_torrent(sha1hash))
            //        using (Core.TorrentStatus ts = th.status())
            //        {
            //            if (ts.has_metadata && ts.need_save_resume)
            //            {
            //                th.save_resume_data(1 | 2 | 4);
            //            }
            //        }
            //        //totConnex += item.NumConnections;
            //    }
            //    //SessionStatistic.NumConnections = totConnex;
            //}
        }

        private void HandleAlertCallback()
        {
            _torrentSession.get_pending_alerts();
        }

        private void HandlePendingAlertCallback(Core.Alert a)
        {
            CoreLog.Trace("libtorrent event {0}: {1}", a.what(), a.message());
            if (!alertType.ContainsKey(a.GetType()))
            {
                return;
            }
            switch (alertType[a.GetType()])
            {
                case 0:
                    TorrentAddedAlert((Core.torrent_added_alert)a);
                    break;
                case 1:
                    StateUpdateAlert((Core.state_update_alert)a);
                    break;
                case 2: // torrent_paused_alert
                    break;
                case 3: // torrent_resumed_alert
                    break;
                case 4:
                    TorrentRemovedAlert((Core.torrent_removed_alert)a);
                    break;
                case 5: // torrent_deleted_alert
                    break;
                case 6: // torrent_error_alert
                    break;
                case 7: // dht_stats_alert
                    break;
                case 8: // dht_bootstrap_alert
                    break;
                case 9:
                    SaveResumeDataAlert((Core.save_resume_data_alert)a);
                    break;
                case 10:
                    SaveResumeDataFailedAlert((Core.save_resume_data_failed_alert)a);
                    break;
                case 11: // piece_finished_alert
                    PieceFinishedAlert((Core.piece_finished_alert)a);
                    break;
                default:
                    break;
            }
        }

        private void PieceFinishedAlert(piece_finished_alert a)
        {
            Interlocked.MemoryBarrier();
            using(Core.TorrentHandle th = a.handle)
            using(Core.Sha1Hash hash = th.info_hash())
            {
                if (TorrentList.ToList().Any(x => x.Hash == hash.ToString()))
                {
                    Models.TorrentItem ti = TorrentList.First(z => z.Hash == hash.ToString());
                    if (!ReferenceEquals(null, ti.Pieces.Parts) && ti.Pieces.Parts.Any(q => q.Id == a.piece_index))
                    {
                        ti.Pieces.Parts[a.piece_index].Downloaded = true;

                        //var pp = th.piece_priorities();

                        //if (ti.ForceSequential)
                        if (ti.SequentialDownload)
                        {
                            //foreach (Models.Part item in ti.Pieces.Parts)
                            //{
                            //    if (!item.Downloaded)
                            //    {
                            //        th.piece_priority(item.Id, 7);
                            //        break;
                            //    }
                            //}
                            if (ti.Pieces.Parts.Any(w => w.Downloaded == false))
                            {
                                Models.Part mp = ti.Pieces.Parts.First(w => w.Downloaded == false);
                                mp.Priority = 7;
                                th.piece_priority(mp.Id, 7);
                            }
                        }

                    }
                }

            }
        }

        private void TorrentRemovedAlert(Core.torrent_removed_alert a)
        {
            using (Core.Sha1Hash sha1hash = a.info_hash)
            {
                string hash = sha1hash.ToString();
                if (TorrentList.ToList().Any(e => e.Hash == hash))
                {
                    Models.TorrentItem ti = TorrentList.ToList().First(f => f.Hash == hash);
                    Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate ()
                    {
                        TorrentList.Remove(ti);
                    });
                }
                if (File.Exists("./Fastresume/" + hash + ".fastresume"))
                {
                    File.Delete("./Fastresume/" + hash + ".fastresume");
                }
                if (File.Exists("./Fastresume/" + hash + ".torrent"))
                {
                    File.Delete("./Fastresume/" + hash + ".torrent");
                }
            }
        }

        private void SaveResumeDataAlert(Core.save_resume_data_alert a)
        {
            TorrentHandle h = null;
            BinaryWriter bw = null;
            FileStream fs = null;
            try
            {
                h = a.handle;
                string newfilePath = ("./Fastresume/" + h.info_hash().ToString() + ".fastresume");
                var data = Core.Util.bencode(a.resume_data);
                fs = new FileStream(newfilePath, FileMode.OpenOrCreate);
                bw = new BinaryWriter(fs);
                bw.Write(data);
                bw.Close();
            }
            finally
            {
                if (!ReferenceEquals(null, h)) h.Dispose();
                if (!ReferenceEquals(null, bw)) bw.Dispose();
                if (!ReferenceEquals(null, fs)) fs.Dispose();
            }
            
            Interlocked.Decrement(ref outstanding_resume_data);
            if (outstanding_resume_data == 0 && no_more_resume)
                no_more_data.Set();
        }

        private void SaveResumeDataFailedAlert(Core.save_resume_data_failed_alert a)
        {
            Interlocked.Decrement(ref outstanding_resume_data);
            if (outstanding_resume_data == 0 && no_more_resume)
                no_more_data.Set();
        }

        public async System.Threading.Tasks.Task LoadFastResumeData()
        {
            IsTsunamiEnabled = false;
            if (Directory.Exists("Fastresume"))
            {
                // FARE TRY CATCH FINALLY

                //Window loading = new Pages.Loading();
                //loading.Show();
                //System.Windows.Controls.Label lb = (System.Windows.Controls.Label)loading.FindName("txtLoading");
                //System.Windows.Controls.ProgressBar pb = (System.Windows.Controls.ProgressBar)loading.FindName("progressLoading");

                string[] files = Directory.GetFiles("Fastresume", "*.fastresume");

                //int i = 0;

                if (files.Length > 0)
                {
                    // fast resuming
                    //pb.Maximum = files.Length+0.01;
                    FileToLoad = files.Length + 0.01;

                    foreach (string s in files)
                    {
                        //i++;
                        FileLoading++;
                        //lb.Content = "Loading "+i+" of "+files.Length+" torrents";
                        StringLoading = "Loading " + FileLoading + " of " + files.Length + " torrents";
                        //pb.Value = i;
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
                            await System.Threading.Tasks.Task.Run(() =>  _torrentSession.add_torrent(atp));
                        }
                        await System.Threading.Tasks.Task.Delay(100);
                    }
                } else
                {
                    // nothing to fast resume, sleep
                    //lb.Content = "Tsunami is loading...";
                    StringLoading = "Tsunami is loading...";
                    //pb.Maximum = 10.001;
                    FileToLoad = 10.001;
                    while (FileLoading < 10)
                    {
                        //    i++;
                        FileLoading++;
                        //    pb.Value = i;
                        await System.Threading.Tasks.Task.Delay(250);
                    }
                }
                //loading.Close();
            }
            else
            {
                Directory.CreateDirectory("Fastresume");
                StringLoading = "Tsunami is loading...";
                FileToLoad = 10.001;
                while (FileLoading < 10)
                {
                    FileLoading++;
                    await System.Threading.Tasks.Task.Delay(250);
                }
            }
            IsTsunamiEnabled = true;
        }

        public void AddClick_Dialog()
        {
            bool atLeastOneFileSelected = false;
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Torrent|*.torrent";
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Select Torrent to Add";
            ofd.ShowDialog();
            foreach (string file in ofd.FileNames)
            {
                atLeastOneFileSelected = true;
                AddTorrent(file);
            }
            if (atLeastOneFileSelected)
            {
                Classes.Switcher.Switch(MainWindow._listPage);
            }
        }

        public void AddTorrent(string filename)
        {
            var data = System.IO.File.ReadAllBytes(filename);
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
                if (!System.IO.File.Exists(newfilePath))
                {
                    using (var bw = new System.IO.BinaryWriter(new System.IO.FileStream(newfilePath, System.IO.FileMode.Create)))
                    {
                        bw.Write(data);
                        bw.Close();
                    }
                }
                _torrentSession.async_add_torrent(atp);
            }
        }

        public void PauseTorrent(string hash)
        {
            using (Core.Sha1Hash sha1hash = new Core.Sha1Hash(hash))
            using (Core.TorrentHandle th = _torrentSession.find_torrent(sha1hash))
            {
                if (th != null  && th.is_valid())
                {
                    th.pause();
                }
            }
        }

        public void ResumeTorrent(string hash)
        {
            using (Core.Sha1Hash sha1hash = new Core.Sha1Hash(hash))
            using (Core.TorrentHandle th = _torrentSession.find_torrent(sha1hash))
            {
                if (th != null && th.is_valid())
                {
                    th.resume();
                }
            }
        }

        public void RemoveTorrent(string hash, bool deleteFileToo = false)
        {
            using (Core.Sha1Hash sha1hash = new Core.Sha1Hash(hash))
            using (Core.TorrentHandle th = _torrentSession.find_torrent(sha1hash))
            {
                if (th != null && th.is_valid())
                {
                    _torrentSession.remove_torrent(th, Convert.ToInt32(deleteFileToo));
                }
            }
        }

        private void TorrentAddedAlert(Core.torrent_added_alert a)
        {
            using (Core.TorrentHandle th = a.handle)
            using (Core.TorrentStatus ts = th.status())
            {
                Models.TorrentItem ti = new Models.TorrentItem(ts);
                ti.PropertyChanged += torrentItem_PropertyChanged;

                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate ()
                    {
                        TorrentList.Add(ti);
                        //Hardcodet.Wpf.TaskbarNotification.TaskbarIcon tbi = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)App.Current.MainWindow.FindName("tsunamiNotifyIcon");
                        //string title = "Tsunami";
                        //string text = "New torrent added!";
                        //tbi.ShowBalloonTip(title, text, tbi.Icon, true);
                    });
            }
        }

        private void torrentItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ForceSequential")
            {
                Models.TorrentItem ti = (Models.TorrentItem)sender;
                using (Core.Sha1Hash hash = new Core.Sha1Hash(ti.Hash))
                using (Core.TorrentHandle th = _torrentSession.find_torrent(hash))
                {
                    if (ti.SequentialDownload)
                    {
                        th.set_sequential_download(true);
                        foreach (Models.Part mp in ti.Pieces.Parts.Where(x => x.Downloaded == false))
                        {
                            mp.Priority = 1;
                            th.piece_priority(mp.Id, 1);
                        }
                        if (ti.Pieces.Parts.Any(w => w.Downloaded == false))
                        {
                            Models.Part mp = ti.Pieces.Parts.First(w => w.Downloaded == false);
                            mp.Priority = 7;
                            th.piece_priority(mp.Id, 7);
                        }
                    } else {
                        th.set_sequential_download(false);
                        foreach (Models.Part mp in ti.Pieces.Parts.Where(x => x.Downloaded == false))
                        {
                            mp.Priority = 4;
                            th.piece_priority(mp.Id, 4);
                        }
                    }
                }
            }
        }

        private void StateUpdateAlert(Core.state_update_alert a)
        {
            foreach (Core.TorrentStatus ts in a.status)
                using (ts)
                using (Core.Sha1Hash hash = ts.info_hash)
                {
                    if (TorrentList.ToList().Any(z => z.Hash == hash.ToString()))
                    {
                        Models.TorrentItem ti = TorrentList.First(e => e.Hash == hash.ToString());
                        ti.Update(ts);
                    }
                }
        }

        public void Terminate()
        {
            _dispatcherTimer.Stop();
            _dispatcherTimer.Enabled = false;

            IsTsunamiEnabled = false;

            _torrentSession.pause();

            //stopWeb();
            TerminateSaveResume();

            outstanding_resume_data = 0;
            List<Models.TorrentItem> myList = new List<Models.TorrentItem>(TorrentList);
            foreach (Models.TorrentItem item in myList)
            {
                using (Core.Sha1Hash sha1hash = new Core.Sha1Hash(item.Hash))
                using (Core.TorrentHandle th = _torrentSession.find_torrent(sha1hash))
                using (Core.TorrentStatus ts = th.status())
                {
                    if (ts.has_metadata && ts.need_save_resume)
                    {
                        ++outstanding_resume_data;
                        th.save_resume_data(1 | 2 | 4);
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            no_more_resume = true;
            //if (outstanding_resume_data > 0)
            if (outstanding_resume_data != 0)
                no_more_data.WaitOne();

            _torrentSession.clear_alert_callback();
        }

        private void TerminateSaveResume()
        {
            using (var entry = _torrentSession.save_state(0xfffffff))
            {
                var data = Core.Util.bencode(entry);
                File.WriteAllBytes(".session_state", data);
            }
            //_torrentSession?.Dispose();
            //_torrentSession = null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _dispatcherTimer?.Dispose();
                    no_more_data?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                _torrentSession?.Dispose();
                Dht?.Dispose();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~TsunamiViewModel()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }


    public class CommandHandler : ICommand
    {
        private readonly Action _action;
        private readonly bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }

}
