using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class PeerInfo
    {
        public int BusyRequests { get; set; }
        public string Client { get; set; }
        public int ConnectionType { get; set; }
        public string Country { get; set; }
        public int DownloadingBlockIndex { get; set; }
        public int DownloadingPieceIndex { get; set; }
        public int DownloadingProgress { get; set; }
        public int DownloadingTotal { get; set; }
        public int DownloadLimit { get; set; }
        public int DownloadQueueLength { get; set; }
        public TimeSpan DownloadQueueTime { get; set; }
        public int DownloadRatePeak { get; set; }
        public int DownSpeed { get; set; }
        public int EstimatedReciprocationRate { get; set; }
        public int Failcount { get; set; }
        public uint Flags { get; set; }
        public string InetAsName { get; set; }
        public TimeSpan LastActive { get; set; }
        public TimeSpan LastRequest { get; set; }
        public int NumHashfails { get; set; }
        public int NumPieces { get; set; }
        public int PayloadDownSpeed { get; set; }
        public int PayloadUpSpeed { get; set; }
        public int PendingDiskbytes { get; set; }
        public int ProgressPpm { get; set; }
        public int QueueBytes { get; set; }
        public int ReceiveBufferSize { get; set; }
        public int ReceiveQuota { get; set; }
        public int RemoteDlRate { get; set; }
        public int RequestsInBuffer { get; set; }
        public int RequestTimeout { get; set; }
        public int Rtt { get; set; }
        public int SendBufferSize { get; set; }
        public int SendQuota { get; set; }
        public int Source { get; set; }
        public int TargetDlQueueLength { get; set; }
        public int TimedOutRequests { get; set; }
        public long TotalDownload { get; set; }
        public long TotalUpload { get; set; }
        public int UploadLimit { get; set; }
        public int UploadQueueLength { get; set; }
        public int UploadRatePeak { get; set; }
        public int UpSpeed { get; set; }
        public int UsedReceiveBuffer { get; set; }
        public int UsedSendBuffer { get; set; }

        public PeerInfo() { /* nothing to do. just for serializator */ }

        public PeerInfo(Core.PeerInfo pi)
        {
            BusyRequests = pi.busy_requests;
            Client = pi.client;
            ConnectionType = pi.connection_type;
            Country = pi.country;
            DownloadingBlockIndex = pi.downloading_block_index;
            DownloadingPieceIndex = pi.downloading_piece_index;
            DownloadingProgress = pi.downloading_progress;
            DownloadingTotal = pi.downloading_total;
            DownloadLimit = pi.download_limit;
            DownloadQueueLength = pi.download_queue_length;
            DownloadQueueTime = pi.download_queue_time;
            DownloadRatePeak = pi.download_rate_peak;
            DownSpeed = pi.down_speed;
            EstimatedReciprocationRate = pi.estimated_reciprocation_rate;
            Failcount = pi.failcount;
            Flags = pi.flags;
            InetAsName = pi.inet_as_name;
            LastActive = pi.last_active;
            LastRequest = pi.last_request;
            NumHashfails = pi.num_hashfails;
            NumPieces = pi.num_pieces;
            PayloadDownSpeed = pi.payload_down_speed;
            PayloadUpSpeed = pi.payload_up_speed;
            PendingDiskbytes = pi.pending_disk_bytes;
            ProgressPpm = pi.progress_ppm;
            QueueBytes = pi.queue_bytes;
            ReceiveBufferSize = pi.receive_buffer_size;
            ReceiveQuota = pi.receive_quota;
            RemoteDlRate = pi.remote_dl_rate;
            RequestsInBuffer = pi.requests_in_buffer;
            RequestTimeout = pi.request_timeout;
            Rtt = pi.rtt;
            SendBufferSize = pi.send_buffer_size;
            SendQuota = pi.send_quota;
            Source = pi.source;
            TargetDlQueueLength = pi.target_dl_queue_length;
            TimedOutRequests = pi.timed_out_requests;
            TotalDownload = pi.total_download;
            TotalUpload = pi.total_upload;
            UploadLimit = pi.upload_limit;
            UploadQueueLength = pi.upload_queue_length;
            UploadRatePeak = pi.upload_rate_peak;
            UpSpeed = pi.up_speed;
            UsedReceiveBuffer = pi.used_receive_buffer;
            UsedSendBuffer = pi.used_send_buffer;
        }
    }
}
