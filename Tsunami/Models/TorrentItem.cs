using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Humanizer;
using System.Collections.ObjectModel;

namespace Tsunami.Models
{
    public class TorrentItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _QueuePosition;
        private string _Name;
        private string _Hash;
        private long _TotalWanted;
        private long _TotalDone;
        private string _State;
        private float _Progress;
        private int _Priority;
        private int _DownloadRate;
        private int _UploadRate;
        private int _NumConnections;

        private int _ActiveTime;
        private DateTime _AddedTime;
        private long _AllTimedownload;
        private long _AllTimeupload;
        private TimeSpan _AnnounceInterval;
        private bool _AutoManaged;
        private int _BlockSize;
        private DateTime _CompletedTime;
        private int _ConnectionsLimit;
        private int _ConnectCandidates;
        private string _CurrentTracker;
        private float _DistributedCopies;
        private int _DistributedFraction;
        private int _DistributedFullcopies;
        private int _DownloadPayloadrate;
        private int _DownBandwidthqueue;
        private string _Error;
        private int _FinishedTime;
        private bool _HasIncoming;
        private bool _HasMetadata;
        private bool _IpFilterapplies;
        private bool _IsFinished;
        private bool _IsSeeding;
        private int _LastScrape;
        private DateTime _LastSeencomplete;
        private int _ListPeers;
        private int _ListSeeds;
        private bool _MovingStorage;
        private bool _NeedSaveresume;
        private TimeSpan _NextAnnounce;
        private int _NumComplete;
        private int _NumIncomplete;
        private int _NumPeers;
        private int _NumPieces;
        private int _NumSeeds;
        private int _NumUploads;
        private bool _Paused;
        private BitField _Pieces;
        private int _ProgressPpm;
        private string _SavePath;
        private int _SeedingTime;
        private bool _SeedMode;
        private int _SeedRank;
        private bool _SequentialDownload;
        private bool _ShareMode;
        private string _StorageMode;
        private bool _SuperSeeding;
        private int _TimeSincedownload;
        private int _TimeSinceupload;
        private long _TotalDownload;
        private long _TotalFailedbytes;
        private long _TotalPayloaddownload;
        private long _TotalPayloadupload;
        private long _TotalReduntantbytes;
        private long _TotalUpload;
        private long _TotalWanteddone;
        private int _UploadsLimit;
        private bool _UploadMode;
        private int _UploadPayloadrate;
        private int _UpBandwidthqueue;
        private BitField _VerifiedPieces;

        private bool _isSelected;
        //private bool _forceSequential;

        public int QueuePosition { get { return _QueuePosition; } set { if (_QueuePosition != value) { _QueuePosition = value; CallPropertyChanged("QueuePosition"); } } }
        public string Name { get { return _Name; } set { if (_Name != value) { _Name = value; CallPropertyChanged("Name"); } } }
        public string Hash { get { return _Hash; } set { if (_Hash != value) { _Hash = value; CallPropertyChanged("Hash"); } } }
        public long TotalWanted { get { return _TotalWanted; } set { if (_TotalWanted != value) { _TotalWanted = value; CallPropertyChanged("TotalWanted_ByteSize"); } } }
        public long TotalDone { get { return _TotalDone; } set { if (_TotalDone != value) { _TotalDone = value; CallPropertyChanged("TotalDone_ByteSize"); CallPropertyChanged("RemainingTime"); CallPropertyChanged("RemainingVerbose"); CallPropertyChanged("Remaining_ByteSize"); } } }
        public string State { get { return _State; } set { if (_State != value) { _State = value; CallPropertyChanged("State"); CallPropertyChanged("State_Image"); CallPropertyChanged("IsFinishedVisible"); CallPropertyChanged("IsDownloadVisible"); } } }
        /* Floating point numbers should not be tested for equality
         * https://www.misra.org.uk/forum/viewtopic.php?t=294 */
        public float Progress {
            get { return _Progress; }
            set {
                //double tolerance = 0.001;
                //if ((value >= (_Progress - tolerance) && (value <= (_Progress + tolerance))))
                //{
                    _Progress = value;
                    CallPropertyChanged("Progress_String");
                    CallPropertyChanged("Progress_Number");
                    CallPropertyChanged("Progress_Color");
                //}
            }
        }
        public int Priority { get { return _Priority; } set { if (_Priority != value) { _Priority = value; CallPropertyChanged("Priority_String"); } } }
        public int DownloadRate { get { return _DownloadRate; } set { if (_DownloadRate != value) { _DownloadRate = value; CallPropertyChanged("DownloadRate_ByteSize"); CallPropertyChanged("DownloadRate_Symbol"); CallPropertyChanged("DownloadRate_Short"); } } }
        public int UploadRate { get { return _UploadRate; } set { if (_UploadRate != value) { _UploadRate = value; CallPropertyChanged("UploadRate_ByteSize"); CallPropertyChanged("UploadRate_Symbol"); CallPropertyChanged("UploadRate_Short"); } } }
        public int NumConnections { get { return _NumConnections; } set { if (_NumConnections != value) { _NumConnections = value; CallPropertyChanged("NumConnections"); } } }

        public int ActiveTime { get { return _ActiveTime; } set { if (_ActiveTime != value) { _ActiveTime = value; CallPropertyChanged("ActiveTime"); } } }
        public DateTime AddedTime { get { return _AddedTime; } set { if (_AddedTime != value) { _AddedTime = value; CallPropertyChanged("AddedTime"); } } }
        public long AllTimeDownload { get { return _AllTimedownload; } set { if (_AllTimedownload != value) { _AllTimedownload = value; CallPropertyChanged("AllTimeDownload"); } } }
        public long AllTimeUpload { get { return _AllTimeupload; } set { if (_AllTimeupload != value) { _AllTimeupload = value; CallPropertyChanged("AllTimeUpload"); } } }
        public TimeSpan AnnounceInterval { get { return _AnnounceInterval; } set { if (_AnnounceInterval != value) { _AnnounceInterval = value; CallPropertyChanged("AnnounceInterval"); } } }
        public bool AutoManaged { get { return _AutoManaged; } set { if (_AutoManaged != value) { _AutoManaged = value; CallPropertyChanged("AutoManaged"); } } }
        public int BlockSize { get { return _BlockSize; } set { if (_BlockSize != value) { _BlockSize = value; CallPropertyChanged("BlockSize"); } } }
        public DateTime CompletedTime { get { return _CompletedTime; } set { if (_CompletedTime != value) { _CompletedTime = value; CallPropertyChanged("CompletedTime"); } } }
        public int ConnectionsLimit { get { return _ConnectionsLimit; } set { if (_ConnectionsLimit != value) { _ConnectionsLimit = value; CallPropertyChanged("ConnectionsLimit"); } } }
        public int ConnectCandidates { get { return _ConnectCandidates; } set { if (_ConnectCandidates != value) { _ConnectCandidates = value; CallPropertyChanged("ConnectCandidates"); } } }
        public string CurrentTracker { get { return _CurrentTracker; } set { if (_CurrentTracker != value) { _CurrentTracker = value; CallPropertyChanged("CurrentTracker"); } } }
        public float DistributedCopies { get { return _DistributedCopies; } set { if (_DistributedCopies != value) { _DistributedCopies = value; CallPropertyChanged("DistributedCopies"); } } }
        public int DistributedFraction { get { return _DistributedFraction; } set { if (_DistributedFraction != value) { _DistributedFraction = value; CallPropertyChanged("DistributedFraction"); } } }
        public int DistributedFullCopies { get { return _DistributedFullcopies; } set { if (_DistributedFullcopies != value) { _DistributedFullcopies = value; CallPropertyChanged("DistributedFullCopies"); } } }
        public int DownloadPayloadRate { get { return _DownloadPayloadrate; } set { if (_DownloadPayloadrate != value) { _DownloadPayloadrate = value; CallPropertyChanged("DownloadPayloadRate"); } } }
        public int DownBandwidthQueue { get { return _DownBandwidthqueue; } set { if (_DownBandwidthqueue != value) { _DownBandwidthqueue = value; CallPropertyChanged("DownBandwidthQueue"); } } }
        public string Error { get { return _Error; } set { if (_Error != value) { _Error = value; CallPropertyChanged("Error"); } } }
        public int FinishedTime { get { return _FinishedTime; } set { if (_FinishedTime != value) { _FinishedTime = value; CallPropertyChanged("FinishedTime"); } } }
        public bool HasIncoming { get { return _HasIncoming; } set { if (_HasIncoming != value) { _HasIncoming = value; CallPropertyChanged("HasIncoming"); } } }
        public bool HasMetadata { get { return _HasMetadata; } set { if (_HasMetadata != value) { _HasMetadata = value; CallPropertyChanged("HasMetadata"); } } }
        public bool IpFilterApplies { get { return _IpFilterapplies; } set { if (_IpFilterapplies != value) { _IpFilterapplies = value; CallPropertyChanged("IpFilterApplies"); } } }
        public bool IsFinished { get { return _IsFinished; } set { if (_IsFinished != value) { _IsFinished = value; CallPropertyChanged("IsFinished"); } } }
        public bool IsSeeding { get { return _IsSeeding; } set { if (_IsSeeding != value) { _IsSeeding = value; CallPropertyChanged("IsSeeding"); } } }
        public int LastScrape { get { return _LastScrape; } set { if (_LastScrape != value) { _LastScrape = value; CallPropertyChanged("LastScrape"); } } }
        public DateTime LastSeenComplete { get { return _LastSeencomplete; } set { if (_LastSeencomplete != value) { _LastSeencomplete = value; CallPropertyChanged("LastSeenComplete"); } } }
        public int ListPeers { get { return _ListPeers; } set { if (_ListPeers != value) { _ListPeers = value; CallPropertyChanged("ListPeers"); } } }
        public int ListSeeds { get { return _ListSeeds; } set { if (_ListSeeds != value) { _ListSeeds = value; CallPropertyChanged("ListSeeds"); } } }
        public bool MovingStorage { get { return _MovingStorage; } set { if (_MovingStorage != value) { _MovingStorage = value; CallPropertyChanged("MovingStorage"); } } }
        public bool NeedSaveResume { get { return _NeedSaveresume; } set { if (_NeedSaveresume != value) { _NeedSaveresume = value; CallPropertyChanged("NeedSaveResume"); } } }
        public TimeSpan NextAnnounce { get { return _NextAnnounce; } set { if (_NextAnnounce != value) { _NextAnnounce = value; CallPropertyChanged("NextAnnounce"); } } }
        public int NumComplete { get { return _NumComplete; } set { if (_NumComplete != value) { _NumComplete = value; CallPropertyChanged("NumComplete"); } } }
        public int NumIncomplete { get { return _NumIncomplete; } set { if (_NumIncomplete != value) { _NumIncomplete = value; CallPropertyChanged("NumIncomplete"); } } }
        public int NumPeers { get { return _NumPeers; } set { if (_NumPeers != value) { _NumPeers = value; CallPropertyChanged("NumPeers"); } } }
        public int NumPieces { get { return _NumPieces; } set { if (_NumPieces != value) { _NumPieces = value; CallPropertyChanged("NumPieces"); } } }
        public int NumSeeds { get { return _NumSeeds; } set { if (_NumSeeds != value) { _NumSeeds = value; CallPropertyChanged("NumSeeds"); } } }
        public int NumUploads { get { return _NumUploads; } set { if (_NumUploads != value) { _NumUploads = value; CallPropertyChanged("NumUploads"); } } }
        public bool Paused { get { return _Paused; } set { if (_Paused != value) { _Paused = value; CallPropertyChanged("Paused"); } } }
        public BitField Pieces { get { return _Pieces; } set { if (_Pieces != value) { _Pieces = value; CallPropertyChanged("Pieces"); } } }
        public int ProgressPpm { get { return _ProgressPpm; } set { if (_ProgressPpm != value) { _ProgressPpm = value; CallPropertyChanged("ProgressPpm"); } } }
        public string SavePath { get { return _SavePath; } set { if (_SavePath != value) { _SavePath = value; CallPropertyChanged("SavePath"); } } }
        public int SeedingTime { get { return _SeedingTime; } set { if (_SeedingTime != value) { _SeedingTime = value; CallPropertyChanged("SeedingTime"); } } }
        public bool SeedMode { get { return _SeedMode; } set { if (_SeedMode != value) { _SeedMode = value; CallPropertyChanged("SeedMode"); } } }
        public int SeedRank { get { return _SeedRank; } set { if (_SeedRank != value) { _SeedRank = value; CallPropertyChanged("SeedRank"); } } }
        public bool SequentialDownload { get { return _SequentialDownload; } set { if (_SequentialDownload != value) { _SequentialDownload = value; CallPropertyChanged("SequentialDownload"); } } }
        public bool ShareMode { get { return _ShareMode; } set { if (_ShareMode != value) { _ShareMode = value; CallPropertyChanged("ShareMode"); } } }
        public string StorageMode { get { return _StorageMode; } set { if (_StorageMode != value) { _StorageMode = value; CallPropertyChanged("StorageMode"); } } }
        public bool SuperSeeding { get { return _SuperSeeding; } set { if (_SuperSeeding != value) { _SuperSeeding = value; CallPropertyChanged("SuperSeeding"); } } }
        public int TimeSinceDownload { get { return _TimeSincedownload; } set { if (_TimeSincedownload != value) { _TimeSincedownload = value; CallPropertyChanged("TimeSinceDownload"); } } }
        public int TimeSinceUpload { get { return _TimeSinceupload; } set { if (_TimeSinceupload != value) { _TimeSinceupload = value; CallPropertyChanged("TimeSinceUpload"); } } }
        public long TotalDownload { get { return _TotalDownload; } set { if (_TotalDownload != value) { _TotalDownload = value; CallPropertyChanged("TotalDownload"); } } }
        public long TotalFailedBytes { get { return _TotalFailedbytes; } set { if (_TotalFailedbytes != value) { _TotalFailedbytes = value; CallPropertyChanged("TotalFailedBytes"); } } }
        public long TotalPayloadDownload { get { return _TotalPayloaddownload; } set { if (_TotalPayloaddownload != value) { _TotalPayloaddownload = value; CallPropertyChanged("TotalPayloadDownload"); } } }
        public long TotalPayloadUpload { get { return _TotalPayloadupload; } set { if (_TotalPayloadupload != value) { _TotalPayloadupload = value; CallPropertyChanged("TotalPayloadUpload"); } } }
        public long TotalReduntantBytes { get { return _TotalReduntantbytes; } set { if (_TotalReduntantbytes != value) { _TotalReduntantbytes = value; CallPropertyChanged("TotalReduntantBytes"); } } }
        public long TotalUpload { get { return _TotalUpload; } set { if (_TotalUpload != value) { _TotalUpload = value; CallPropertyChanged("TotalUpload"); } } }
        public long TotalWantedDone { get { return _TotalWanteddone; } set { if (_TotalWanteddone != value) { _TotalWanteddone = value; CallPropertyChanged("TotalWantedDone"); } } }
        public int UploadsLimit { get { return _UploadsLimit; } set { if (_UploadsLimit != value) { _UploadsLimit = value; CallPropertyChanged("UploadsLimit"); } } }
        public bool UploadMode { get { return _UploadMode; } set { if (_UploadMode != value) { _UploadMode = value; CallPropertyChanged("UploadMode"); } } }
        public int UploadPayloadRate { get { return _UploadPayloadrate; } set { if (_UploadPayloadrate != value) { _UploadPayloadrate = value; CallPropertyChanged("UploadPayloadRate"); } } }
        public int UpBandwidthQueue { get { return _UpBandwidthqueue; } set { if (_UpBandwidthqueue != value) { _UpBandwidthqueue = value; CallPropertyChanged("UpBandwidthQueue"); } } }
        public BitField VerifiedPieces { get { return _VerifiedPieces; } set { if (_VerifiedPieces != value) { _VerifiedPieces = value; CallPropertyChanged("VerifiedPieces"); } } }

        public ObservableCollection<Models.FileEntry> FileList { get; set; }

        public bool IsSelected { get { return _isSelected; } set { if (_isSelected != value) { _isSelected = value; CallPropertyChanged("IsSelected"); } } }

        public System.Windows.Visibility IsPauseButtonVisible {
            get
            {
                if (State != "Downloading")
                {
                    return System.Windows.Visibility.Collapsed;
                } else
                {
                    return System.Windows.Visibility.Visible;
                }
            }
        }
        public System.Windows.Visibility IsResumeButtonVisible
        {
            get
            {
                if (State == "Paused")
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }
        }

        public Func<double, string> Formatter { get; set; }

        public string TotalWanted_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_TotalWanted);
                return _TotalWanted.Bytes().ToString("0.00");
            }
        }
        public string TotalDone_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_TotalDone);
                return _TotalDone.Bytes().ToString("0.00");
            }
        }

        public string DownloadRate_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_DownloadRate) + @"/s";
                return _DownloadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Humanize("0.00");
            }
        }
        public string DownloadRate_Symbol
        {
            get
            {
                return _DownloadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Size.LargestWholeNumberSymbol.ToUpper();
            }
        }
        public string DownloadRate_Short
        {
            get
            {
                return _DownloadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Size.LargestWholeNumberValue.ToString("#");
            }
        }

        public string UploadRate_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_UploadRate) + @"/s";
                return _UploadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Humanize("0.00");
            }
        }
        public string UploadRate_Symbol
        {
            get
            {
                return _UploadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Size.LargestWholeNumberSymbol.ToUpper();
            }
        }
        public string UploadRate_Short
        {
            get
            {
                return _UploadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Size.LargestWholeNumberValue.ToString("#");
            }
        }

        public string Priority_String
        {
            get
            {
                if (Priority <= 100)
                {
                    return "Normal";
                }
                else if (Priority > 100 && Priority <= 200)
                {
                    return "High";
                }
                else
                {
                    return "Highest";
                }
            }
        }

        public string RemainingTime
        {
            get
            {
                if (DownloadRate == 0) return "∞";
                return string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds((TotalWanted - TotalDone) / (double)DownloadRate));
            }
        }
        public string RemainingVerbose
        {
            get
            {
                if (DownloadRate == 0) return "∞";
                return TimeSpan.FromSeconds((TotalWanted - TotalDone) / (double)DownloadRate).Humanize(3, maxUnit: Humanizer.Localisation.TimeUnit.Hour);
            }
        }
        public string Remaining_ByteSize
        {
            get
            {
                return (TotalWanted - TotalDone).Bytes().ToString("0.00");
            }
        }

        public string ShortName
        {
            get
            {
                return _Name.Truncate(18);
            }
        }

        public string Progress_String
        {
            get
            {
                return (Progress * 100).ToString("0.00") + "%";
            }
        }
        public float Progress_Number
        {
            get
            {
                return (Progress * 100);
            }
        }
        public Brush Progress_Color
        {
            get
            {
                Color cl = new Color() {
                    A = 255,
                    R = (byte)((255 * (100 - Progress_Number)) / 100),
                    G = 255,
                    B = (byte)((255 * (100 - Progress_Number)) / 100)
                };
                //cl.A = 255;
                //cl.R = (byte)((255 * (100 - Progress_Number)) / 100);
                //cl.G = (byte)((255 * Progress_Number) / 100);
                //cl.B = (byte)((255 * (100 - Progress_Number)) / 100);
                return new SolidColorBrush(cl);
            }
        }

        public string State_Image
        {
            get
            {
                switch (State)
                {
                    case "Queued For Checking":
                        return "/Resources/state_loading.png";
                    case "Checking Files":
                        return "/Resources/state_loading.png";
                    case "Downloading Metadata":
                        return "/Resources/state_download.png";
                    case "Downloading":
                        return "/Resources/state_download.png";
                    case "Finished":
                        return "/Resources/state_loading.png";
                    case "Seeding":
                        return "/Resources/state_loading.png";
                    case "Allocating":
                        return "/Resources/state_loading.png";
                    case "Checking Resume Data":
                        return "/Resources/state_warning.png";
                    case "Paused":
                        return "/Resources/state_pause.png";
                    default:
                        return "/Resources/state_warning.png";
                }
            }
        }

        public System.Windows.Visibility IsFinishedVisible
        {
            get
            {
                if (State == "Finished" || State == "Seeding")
                {
                    return System.Windows.Visibility.Visible;
                } else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }
        public System.Windows.Visibility IsDownloadVisible
        {
            get
            {
                if (State == "Finished" || State == "Seeding")
                {
                    return System.Windows.Visibility.Hidden;
                }
                else
                {
                    return System.Windows.Visibility.Visible;
                }
            }
        }

        public Color ColorFrom
        {
            get
            {
                //Tuple<MahApps.Metro.AppTheme, MahApps.Metro.Accent> appStyle = MahApps.Metro.ThemeManager.DetectAppStyle(System.Windows.Application.Current);
                //SolidColorBrush scb;
                //if (appStyle.Item1.Name == "BaseDark")
                //{
                //    scb = new SolidColorBrush(Colors.White);
                //} else
                //{
                //    scb = new SolidColorBrush(Colors.Black);
                //}
                //return scb.Color;
                //return Colors.White;
                return new Color() { A = 255, B = 255, G = 255, R = 255 };
            }
        }
        public Color ColorTo
        {
            get
            {
                //Tuple<MahApps.Metro.AppTheme, MahApps.Metro.Accent> appStyle = MahApps.Metro.ThemeManager.DetectAppStyle(System.Windows.Application.Current);
                //SolidColorBrush scb = (SolidColorBrush)appStyle.Item2.Resources["HighlightBrush"];
                //return scb.Color;
                //return new SolidColorBrush(Colors.LimeGreen).Color;
                return new Color() { A = 255, B = 18, G = 253, R = 7 };
            }
        }

        public TorrentItem()
        {
            Formatter = x => ((int)x).ToString() + "%";
            FileList = new ObservableCollection<FileEntry>();
        }

        public TorrentItem(Core.TorrentStatus ts)
        {
            Formatter = x => ((int)x).ToString() + "%";
            FileList = new ObservableCollection<FileEntry>();

            FileEntry fe;
            using (Core.TorrentInfo tf = ts.torrent_file())
            using (Core.FileStorage fs = tf.files())
            {
                // per ogni file nel torrent
                for (int i = 0; i <= tf.num_files() - 1; i++)
                    
                    using (Core.FileEntry cfe = fs.at(i))
                    using (Core.Sha1Hash hash = cfe.filehash)
                    {
                        // lo inserisco
                        fe = new FileEntry(cfe);
                        fe.FileName = fs.file_name(i);
                        fe.IsValid = fs.is_valid();
                        fe.PieceSize = fs.piece_size(i);
                        FileList.Add(fe);
                    }
            }
            if (FileList.Count == 0)
            {
                // non ci sono file nel torrent
                fe = new FileEntry();
                fe.FileName = ts.name;
                FileList.Add(fe);
            }

            SequentialDownload = ts.sequential_download;
            Update(ts);

            //System.Windows.Application.Current.Dispatcher.BeginInvoke(
            //    System.Windows.Threading.DispatcherPriority.Normal,
            //    (Action)delegate ()
            //    {
                    
            //    });

        }

        public void Update(Core.TorrentStatus ts)
        {
            Name = ts.name;
            Priority = ts.priority;
            QueuePosition = ts.queue_position;
            TotalWanted = ts.total_wanted;
            TotalDone = ts.total_done;
            Progress = ts.progress;
            DownloadRate = ts.download_rate;
            UploadRate = ts.upload_rate;

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
            DownBandwidthQueue = ts.down_bandwidth_queue;
            Error = ts.error;
            FinishedTime = ts.finished_time;
            Hash = ts.info_hash.ToString();
            HasIncoming = ts.has_incoming;
            HasMetadata = ts.has_metadata;
            IpFilterApplies = ts.ip_filter_applies;
            IsFinished = ts.is_finished;
            IsSeeding = ts.is_seeding;
            LastScrape = ts.last_scrape;
            LastSeenComplete = ts.last_seen_complete;
            ListPeers = ts.list_peers;
            ListSeeds = ts.list_seeds;
            MovingStorage = ts.moving_storage;
            NeedSaveResume = ts.need_save_resume;
            NextAnnounce = ts.next_announce;
            NumComplete = ts.num_complete;
            NumIncomplete = ts.num_incomplete;
            NumPeers = ts.num_peers;
            NumPieces = ts.num_pieces;
            NumSeeds = ts.num_seeds;
            NumUploads = ts.num_uploads;
            Paused = ts.paused;
            using (Core.BitField bf = ts.pieces)
            {
                if (ReferenceEquals(null, Pieces))
                {
                    Pieces = new BitField(bf);
                } else
                {
                    Pieces.Update(bf);
                }
            }
            //Pieces = new BitField(ts.pieces);
            ProgressPpm = ts.progress_ppm;
            SavePath = ts.save_path;
            SeedingTime = ts.seeding_time;
            SeedMode = ts.seed_mode;
            SeedRank = ts.seed_rank;
            //SequentialDownload = ts.sequential_download;
            ShareMode = ts.share_mode;
            State = (ts.paused) ? "Paused" : Classes.Utils.GiveMeStateFromEnum(ts.state);
            StorageMode = Classes.Utils.GiveMeStorageModeFromEnum(ts.storage_mode);
            SuperSeeding = ts.super_seeding;
            TimeSinceDownload = ts.time_since_download;
            TimeSinceUpload = ts.time_since_upload;
            TotalDownload = ts.total_download;
            TotalFailedBytes = ts.total_failed_bytes;
            TotalPayloadDownload = ts.total_payload_download;
            TotalPayloadUpload = ts.total_payload_upload;
            TotalReduntantBytes = ts.total_reduntant_bytes;
            TotalUpload = ts.total_upload;
            TotalWantedDone = ts.total_wanted_done;
            UploadsLimit = ts.uploads_limit;
            UploadMode = ts.upload_mode;
            UploadPayloadRate = ts.upload_payload_rate;
            UpBandwidthQueue = ts.up_bandwidth_queue;
            using (Core.BitField vp = ts.verified_pieces)
            {
                if (ReferenceEquals(null, VerifiedPieces))
                {
                    VerifiedPieces = new BitField(vp);
                }
                else
                {
                    VerifiedPieces.Update(vp);
                }
            }
            //VerifiedPieces = new BitField(ts.verified_pieces);
            //if (ForceSequential && !Pieces.AllSet)
            //{
            //    foreach (Models.Part item in Pieces.Parts)
            //    {
            //        if (!item.Downloaded)
            //        {
            //            ts.handle().piece_priority(item.Id, 7);
            //            break;
            //        }
            //    }
            //}
        }

        public void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

}
