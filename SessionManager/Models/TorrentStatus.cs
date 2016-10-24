using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Tsunami.Models
{
    public class TorrentStatus// : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        //[JsonIgnore]
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
        //public int DownloadRate { get { return DownloadRate; } set { if (DownloadRate != value) { DownloadRate = value; CallPropertyChanged("DownloadRate"); } } }
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
        public BitField Pieces { get; set; }
        //public int Priority { get { return Priority; } set { if (Priority != value) { Priority = value; CallPropertyChanged("Priority"); } } }
        public int Priority { get; set; }
        //public float Progress { get { return Progress; } set { if (Progress != value) { Progress = value; CallPropertyChanged("Progress"); } } }
        public float Progress { get; set; }
        public int ProgressPpm { get; set; }
        //public int QueuePosition { get { return QueuePosition; } set { if (QueuePosition != value) { QueuePosition = value; CallPropertyChanged("QueuePosition"); } } }
        public int QueuePosition { get; set; }
        public string SavePath { get; set; }
        public int SeedingTime { get; set; }
        public bool SeedMode { get; set; }
        public int SeedRank { get; set; }
        public bool SequentialDownload { get; set; }
        public bool ShareMode { get; set; }
        //public string State { get { return State; } set { if (State != value) { State = value; CallPropertyChanged("State"); } } }
        public string State { get; set; }
        public string StorageMode { get; set; }
        public bool SuperSeeding { get; set; }
        public int TimeSinceDownload { get; set; }
        public int TimeSinceUpload { get; set; }
        //public long TotalDone { get { return TotalDone; } set { if (TotalDone != value) { TotalDone = value; CallPropertyChanged("TotalDone"); } } }
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
        //public int UploadRate { get { return UploadRate; } set { if (UploadRate != value) { UploadRate = value; CallPropertyChanged("UploadRate"); } } }
        public int UploadRate { get; set; }
        public int UpBandwidthQueue { get; set; }
        public BitField VerifiedPieces { get; set; }

        public TorrentStatus() { /* nothing to do. just for serializator */ }

        public TorrentStatus(Core.TorrentStatus ts)
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
            Pieces = new BitField(ts.pieces);
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
            State = Utils.GiveMeStateFromEnum(ts.state);
            StorageMode = Utils.GiveMeStorageModeFromEnum(ts.storage_mode);
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
            VerifiedPieces = new BitField(ts.verified_pieces);
            //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InfoHash"));
        }

        //private void CallPropertyChanged(string prop)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}

    }
}
