using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Tsunami.Gui.Wpf
{
    public class SessionStatistics : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _allowedUploadSlots;
        private int _dhtDownloadRate;
        private long _dhtGlobalNodes;
        private int _dhtNodes;
        private int _dhtNodeCache;
        private int _dhtTorrents;
        private int _dhtTotalAllocations;
        private int _dhtUploadRate;
        private int _diskReadQueue;
        private int _diskWriteQueue;
        private int _downloadRate;
        private int _downBandwidthBytesQueue;
        private int _downBandwidthQueue;
        private bool _hasIncomingConnections;
        private int _ipOverheadDownloadRate;
        private int _ipOverheadUploadRate;
        private int _numPeers;
        private int _numUnchoked;
        private int _optimisticUnchokeCounter;
        private int _payloadDownloadRate;
        private int _payloadUploadRate;
        private int _peerlistSize;
        private long _totalDhtDownload;
        private long _totalDhtUpload;
        private long _totalDownload;
        private long _totalFailedBytes;
        private long _totalIpOverheadDownload;
        private long _totalIpOverheadUpload;
        private long _totalPayloadDownload;
        private long _totalPayloadUpload;
        private long _totalRedundantBytes;
        private long _totalTrackerDownload;
        private long _totalTrackerUpload;
        private long _totalUpload;
        private int _trackerDownloadRate;
        private int _trackerUploadRate;
        private int _unchokeCounter;
        private int _uploadRate;
        private int _upBandwidthBytesQueue;
        private int _upBandwidthQueue;

        public int AllowedUploadSlots { get { return _allowedUploadSlots; } set { if (_allowedUploadSlots != value) { _allowedUploadSlots = value; CallPropertyChanged("AllowedUploadSlots"); } } }
        public int DhtDownloadRate { get { return _dhtDownloadRate; } set { if (_dhtDownloadRate != value) { _dhtDownloadRate = value; CallPropertyChanged("DhtDownloadRate"); } } }
        public long DhtGlobalNodes { get { return _dhtGlobalNodes; } set { if (_dhtGlobalNodes != value) { _dhtGlobalNodes = value; CallPropertyChanged("DhtGlobalNodes"); } } }
        public int DhtNodes { get { return _dhtNodes; } set { if (_dhtNodes != value) { _dhtNodes = value; CallPropertyChanged("DhtNodes"); } } }
        public int DhtNodeCache { get { return _dhtNodeCache; } set { if (_dhtNodeCache != value) { _dhtNodeCache = value; CallPropertyChanged("DhtNodeCache"); } } }
        public int DhtTorrents { get { return _dhtTorrents; } set { if (_dhtTorrents != value) { _dhtTorrents = value; CallPropertyChanged("DhtTorrents"); } } }
        public int DhtTotalAllocations { get { return _dhtTotalAllocations; } set { if (_dhtTotalAllocations != value) { _dhtTotalAllocations = value; CallPropertyChanged("DhtTotalAllocations"); } } }
        public int DhtUploadRate { get { return _dhtUploadRate; } set { if (_dhtUploadRate != value) { _dhtUploadRate = value; CallPropertyChanged("DhtUploadRate"); } } }
        public int DiskReadQueue { get { return _diskReadQueue; } set { if (_diskReadQueue != value) { _diskReadQueue = value; CallPropertyChanged("DiskReadQueue"); } } }
        public int DiskWriteQueue { get { return _diskWriteQueue; } set { if (_diskWriteQueue != value) { _diskWriteQueue = value; CallPropertyChanged("DiskWriteQueue"); } } }
        public int DownloadRate { get { return _downloadRate; } set { if (_downloadRate != value) { _downloadRate = value; CallPropertyChanged("DownloadRate"); ; CallPropertyChanged("DownloadRate_ByteSize"); } } }
        public int DownBandwidthBytesQueue { get { return _downBandwidthBytesQueue; } set { if (_downBandwidthBytesQueue != value) { _downBandwidthBytesQueue = value; CallPropertyChanged("DownBandwidthBytesQueue"); } } }
        public int DownBandwidthQueue { get { return _downBandwidthQueue; } set { if (_downBandwidthQueue != value) { _downBandwidthQueue = value; CallPropertyChanged("DownBandwidthQueue"); } } }
        public bool HasIncomingConnections { get { return _hasIncomingConnections; } set { if (_hasIncomingConnections != value) { _hasIncomingConnections = value; CallPropertyChanged("HasIncomingConnections"); } } }
        public int IpOverheadDownloadRate { get { return _ipOverheadDownloadRate; } set { if (_ipOverheadDownloadRate != value) { _ipOverheadDownloadRate = value; CallPropertyChanged("IpOverheadDownloadRate "); } } }
        public int IpOverheadUploadRate { get { return _ipOverheadUploadRate; } set { if (_ipOverheadUploadRate != value) { _ipOverheadUploadRate = value; CallPropertyChanged("IpOverheadUploadRate"); } } }
        public int NumPeers { get { return _numPeers; } set { if (_numPeers != value) { _numPeers = value; CallPropertyChanged("NumPeers"); } } }
        public int NumUnchoked { get { return _numUnchoked; } set { if (_numUnchoked != value) { _numUnchoked = value; CallPropertyChanged("NumUnchoked"); } } }
        public int OptimisticUnchokeCounter { get { return _optimisticUnchokeCounter; } set { if (_optimisticUnchokeCounter != value) { _optimisticUnchokeCounter = value; CallPropertyChanged("OptimisticUnchokeCounter"); } } }
        public int PayloadDownloadRate { get { return _payloadDownloadRate; } set { if (_payloadDownloadRate != value) { _payloadDownloadRate = value; CallPropertyChanged("PayloadDownloadRate"); } } }
        public int PayloadUploadRate { get { return _payloadUploadRate; } set { if (_payloadUploadRate != value) { _payloadUploadRate = value; CallPropertyChanged("PayloadUploadRate"); } } }
        public int PeerlistSize { get { return _peerlistSize; } set { if (_peerlistSize != value) { _peerlistSize = value; CallPropertyChanged("PeerlistSize"); } } }
        public long TotalDhtDownload { get { return _totalDhtDownload; } set { if (_totalDhtDownload != value) { _totalDhtDownload = value; CallPropertyChanged("TotalDhtDownload"); } } }
        public long TotalDhtUpload { get { return _totalDhtUpload; } set { if (_totalDhtUpload != value) { _totalDhtUpload = value; CallPropertyChanged("TotalDhtUpload"); } } }
        public long TotalDownload { get { return _totalDownload; } set { if (_totalDownload != value) { _totalDownload = value; CallPropertyChanged("TotalDownload"); ; CallPropertyChanged("TotalDownload_ByteSize"); } } }
        public long TotalFailedBytes { get { return _totalFailedBytes; } set { if (_totalFailedBytes != value) { _totalFailedBytes = value; CallPropertyChanged("TotalFailedBytes"); } } }
        public long TotalIpOverheadDownload { get { return _totalIpOverheadDownload; } set { if (_totalIpOverheadDownload != value) { _totalIpOverheadDownload = value; CallPropertyChanged("TotalIpOverheadDownload"); } } }
        public long TotalIpOverheadUpload { get { return _totalIpOverheadUpload; } set { if (_totalIpOverheadUpload != value) { _totalIpOverheadUpload = value; CallPropertyChanged("TotalIpOverheadUpload"); } } }
        public long TotalPayloadDownload { get { return _totalPayloadDownload; } set { if (_totalPayloadDownload != value) { _totalPayloadDownload = value; CallPropertyChanged("TotalPayloadDownload"); } } }
        public long TotalPayloadUpload { get { return _totalPayloadUpload; } set { if (_totalPayloadUpload != value) { _totalPayloadUpload = value; CallPropertyChanged("TotalPayloadUpload"); } } }
        public long TotalRedundantBytes { get { return _totalRedundantBytes; } set { if (_totalRedundantBytes != value) { _totalRedundantBytes = value; CallPropertyChanged("TotalRedundantBytes"); } } }
        public long TotalTrackerDownload { get { return _totalTrackerDownload; } set { if (_totalTrackerDownload != value) { _totalTrackerDownload = value; CallPropertyChanged("TotalTrackerDownload"); } } }
        public long TotalTrackerUpload { get { return _totalTrackerUpload; } set { if (_totalTrackerUpload != value) { _totalTrackerUpload = value; CallPropertyChanged("TotalTrackerUpload"); } } }
        public long TotalUpload { get { return _totalUpload; } set { if (_totalUpload != value) { _totalUpload = value; CallPropertyChanged("TotalUpload"); ; CallPropertyChanged("TotalUpload_ByteSize"); } } }
        public int TrackerDownloadRate { get { return _trackerDownloadRate; } set { if (_trackerDownloadRate != value) { _trackerDownloadRate = value; CallPropertyChanged("TrackerDownloadRate"); } } }
        public int TrackerUploadRate { get { return _trackerUploadRate; } set { if (_trackerUploadRate != value) { _trackerUploadRate = value; CallPropertyChanged("TrackerUploadRate"); } } }
        public int UnchokeCounter { get { return _unchokeCounter; } set { if (_unchokeCounter != value) { _unchokeCounter = value; CallPropertyChanged("UnchokeCounter"); } } }
        public int UploadRate { get { return _uploadRate; } set { if (_uploadRate != value) { _uploadRate = value; CallPropertyChanged("UploadRate"); } } }
        public int UpBandwidthBytesQueue { get { return _upBandwidthBytesQueue; } set { if (_upBandwidthBytesQueue != value) { _upBandwidthBytesQueue = value; CallPropertyChanged("UpBandwidthBytesQueue"); } } }
        public int UpBandwidthQueue { get { return _upBandwidthQueue; } set { if (_upBandwidthQueue != value) { _upBandwidthQueue = value; CallPropertyChanged("UpBandwidthQueue"); } } }

        public string TotalDownload_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_totalDownload);
            }
        }

        public string TotalUpload_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_totalUpload);
            }
        }

        public string DownloadRate_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_downloadRate) + @"/s";
            }
        }

        public string UploadRate_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_uploadRate) + @"/s";
            }
        }

        public SessionStatistics()
        {

        }

        public void Update(EventsArgs.OnSessionStatisticsEventArgs es)
        {
            AllowedUploadSlots = es.AllowedUploadSlots;
            DhtDownloadRate = es.DhtDownloadRate;
            DhtGlobalNodes = es.DhtGlobalNodes;
            DhtNodes = es.DhtNodes;
            DhtNodeCache = es.DhtNodeCache;
            DhtTorrents = es.DhtTorrents;
            DhtTotalAllocations = es.DhtTotalAllocations;
            DhtUploadRate = es.DhtUploadRate;
            DiskReadQueue = es.DiskReadQueue;
            DiskWriteQueue = es.DiskWriteQueue;
            DownloadRate = es.DownloadRate;
            DownBandwidthBytesQueue = es.DownBandwidthBytesQueue;
            DownBandwidthQueue = es.DownBandwidthQueue;
            HasIncomingConnections = es.HasIncomingConnections;
            IpOverheadDownloadRate = es.IpOverheadDownloadRate;
            IpOverheadUploadRate = es.IpOverheadUploadRate;
            NumPeers = es.NumPeers;
            NumUnchoked = es.NumUnchoked;
            OptimisticUnchokeCounter = es.OptimisticUnchokeCounter;
            PayloadDownloadRate = es.PayloadDownloadRate;
            PayloadUploadRate = es.PayloadUploadRate;
            PeerlistSize = es.PeerlistSize;
            TotalDhtDownload = es.TotalDhtDownload;
            TotalDhtUpload = es.TotalDhtUpload;
            TotalDownload = es.TotalDownload;
            TotalFailedBytes = es.TotalFailedBytes;
            TotalIpOverheadDownload = es.TotalIpOverheadDownload;
            TotalIpOverheadUpload = es.TotalIpOverheadUpload;
            TotalPayloadDownload = es.TotalPayloadDownload;
            TotalPayloadUpload = es.TotalPayloadUpload;
            TotalRedundantBytes = es.TotalRedundantBytes;
            TotalTrackerDownload = es.TotalTrackerDownload;
            TotalTrackerUpload = es.TotalTrackerUpload;
            TotalUpload = es.TotalUpload;
            TrackerDownloadRate = es.TrackerDownloadRate;
            TrackerUploadRate = es.TrackerUploadRate;
            UnchokeCounter = es.UnchokeCounter;
            UploadRate = es.UploadRate;
            UpBandwidthBytesQueue = es.UpBandwidthBytesQueue;
            UpBandwidthQueue = es.UpBandwidthQueue;
        }

        private void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
