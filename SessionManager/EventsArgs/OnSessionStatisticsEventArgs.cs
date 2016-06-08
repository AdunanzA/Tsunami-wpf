using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.EventsArgs
{
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
