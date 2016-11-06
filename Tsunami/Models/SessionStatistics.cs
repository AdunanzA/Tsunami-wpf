using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Configurations;
using Humanizer;

namespace Tsunami
{
    public class SessionStatistics : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private
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
        private int _maxDownloadRate;
        private double _angularGaugeValue;
        private int _numConnections;
        private bool _isRefreshable;
        #endregion

        #region public
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
        public int DownloadRate { get { return _downloadRate; } set { if (_downloadRate != value) { _downloadRate = value; CallPropertyChanged("DownloadRate"); CallPropertyChanged("TotalDownloadRate_ByteSize"); } } }
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
        public long TotalDownload { get { return _totalDownload; } set { if (_totalDownload != value) { _totalDownload = value; CallPropertyChanged("TotalDownload"); CallPropertyChanged("TotalDownload_ByteSize"); } } }
        public long TotalFailedBytes { get { return _totalFailedBytes; } set { if (_totalFailedBytes != value) { _totalFailedBytes = value; CallPropertyChanged("TotalFailedBytes"); } } }
        public long TotalIpOverheadDownload { get { return _totalIpOverheadDownload; } set { if (_totalIpOverheadDownload != value) { _totalIpOverheadDownload = value; CallPropertyChanged("TotalIpOverheadDownload"); } } }
        public long TotalIpOverheadUpload { get { return _totalIpOverheadUpload; } set { if (_totalIpOverheadUpload != value) { _totalIpOverheadUpload = value; CallPropertyChanged("TotalIpOverheadUpload"); } } }
        public long TotalPayloadDownload { get { return _totalPayloadDownload; } set { if (_totalPayloadDownload != value) { _totalPayloadDownload = value; CallPropertyChanged("TotalPayloadDownload"); } } }
        public long TotalPayloadUpload { get { return _totalPayloadUpload; } set { if (_totalPayloadUpload != value) { _totalPayloadUpload = value; CallPropertyChanged("TotalPayloadUpload"); } } }
        public long TotalRedundantBytes { get { return _totalRedundantBytes; } set { if (_totalRedundantBytes != value) { _totalRedundantBytes = value; CallPropertyChanged("TotalRedundantBytes"); } } }
        public long TotalTrackerDownload { get { return _totalTrackerDownload; } set { if (_totalTrackerDownload != value) { _totalTrackerDownload = value; CallPropertyChanged("TotalTrackerDownload"); } } }
        public long TotalTrackerUpload { get { return _totalTrackerUpload; } set { if (_totalTrackerUpload != value) { _totalTrackerUpload = value; CallPropertyChanged("TotalTrackerUpload"); } } }
        public long TotalUpload { get { return _totalUpload; } set { if (_totalUpload != value) { _totalUpload = value; CallPropertyChanged("TotalUpload"); CallPropertyChanged("TotalUpload_ByteSize"); } } }
        public int TrackerDownloadRate { get { return _trackerDownloadRate; } set { if (_trackerDownloadRate != value) { _trackerDownloadRate = value; CallPropertyChanged("TrackerDownloadRate"); } } }
        public int TrackerUploadRate { get { return _trackerUploadRate; } set { if (_trackerUploadRate != value) { _trackerUploadRate = value; CallPropertyChanged("TrackerUploadRate"); } } }
        public int UnchokeCounter { get { return _unchokeCounter; } set { if (_unchokeCounter != value) { _unchokeCounter = value; CallPropertyChanged("UnchokeCounter"); } } }
        public int UploadRate { get { return _uploadRate; } set { if (_uploadRate != value) { _uploadRate = value; CallPropertyChanged("UploadRate"); CallPropertyChanged("TotalUploadRate_ByteSize"); } } }
        public int UpBandwidthBytesQueue { get { return _upBandwidthBytesQueue; } set { if (_upBandwidthBytesQueue != value) { _upBandwidthBytesQueue = value; CallPropertyChanged("UpBandwidthBytesQueue"); } } }
        public int UpBandwidthQueue { get { return _upBandwidthQueue; } set { if (_upBandwidthQueue != value) { _upBandwidthQueue = value; CallPropertyChanged("UpBandwidthQueue"); } } }
        public int MaxDownloadRate { get { return _maxDownloadRate; } set { if (_maxDownloadRate != value && value > _maxDownloadRate) { _maxDownloadRate = value; CallPropertyChanged("MaxDownloadRate"); } } }
        public int NumConnections { get { return _numConnections; } set { if (_numConnections != value) { _numConnections = value; CallPropertyChanged("NumConnections"); } } }
        public bool IsRefreshable { get { return _isRefreshable; } set { if (_isRefreshable != value) { _isRefreshable = value; CallPropertyChanged("IsRefreshable"); } } }

        /* Floating point numbers should not be tested for equality
         * https://www.misra.org.uk/forum/viewtopic.php?t=294 */
        public double AngularGaugeValue
        {
            get { return _angularGaugeValue; }
            set
            {
                //double tolerance = 0.001F;
                //if ((value >= (_angularGaugeValue - tolerance) && (value <= (_angularGaugeValue + tolerance))))
                //{ 
                    _angularGaugeValue = value;
                    CallPropertyChanged("AngularGaugeValue");
                //}
            }
        }

        public string TotalDownload_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_totalDownload);
                return _totalDownload.Bytes().ToString("0.00");
                
            }
        }

        public string TotalUpload_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_totalUpload);
                return _totalUpload.Bytes().ToString("0.00");
            }
        }

        public string TotalDownloadRate_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_downloadRate) + @"/s";
                return _downloadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Humanize("0.00");
            }
        }

        public string TotalUploadRate_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_uploadRate) + @"/s";
                return _uploadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Humanize("0.00");
            }
        }
        #endregion

        public SessionStatistics()
        {
            //To handle live data easily, in this case we built a specialized type
            //the MeasureModel class, it only contains 2 properties
            //DateTime and Value
            //We need to configure LiveCharts to handle MeasureModel class
            //The next code configures MEasureModel  globally, this means
            //that livecharts learns to plot MeasureModel and will use this config every time
            //a ChartValues instance uses this type.
            //this code ideally should only run once, when application starts is reccomended.
            //you can configure series in many ways, learn more at http://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the values property will store our values array
            DownloadChartValues = new ChartValues<MeasureModel>();
            UploadChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)value).ToString("hh:mm:ss");

            // the y label
            //SizeFormatter = value => Utils.StrFormatByteSize((long)value).Replace("0 bytes", "0");
            SizeFormatter = value => value.Bytes().ToString("#.##");

            XAxisStep = TimeSpan.FromSeconds(1).Ticks;

            YAxisMax = 0.1;
            SetAxisLimits(DateTime.Now);

            MaxDownloadRate = 1;
            NumConnections = 0;

            //Update graphics
            IsRefreshable = true;
            System.Windows.Forms.Timer chartTimer = new System.Windows.Forms.Timer();
            chartTimer.Interval = 5000; //5 seconds
            chartTimer.Tick += new EventHandler(RefreshTimer);
            chartTimer.Start();
        }

        public void Update(Core.SessionStatus ss)
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
            MaxDownloadRate = ss.download_rate;
            AngularGaugeValue = (ss.download_rate.Megabytes().Megabytes/Math.Pow(10,6))*8;

            if (IsRefreshable)
            {
                var now = DateTime.Now;

                DownloadChartValues.Add(new MeasureModel
                {
                    DateTime = now,
                    Value = DownloadRate
                });

                UploadChartValues.Add(new MeasureModel
                {
                    DateTime = now,
                    Value = UploadRate
                });

                SetAxisLimits(now);

                //lets only use the last 30 values (61)
                if (DownloadChartValues.Count > 361) DownloadChartValues.RemoveAt(0);
                if (UploadChartValues.Count > 361) UploadChartValues.RemoveAt(0);

                DownloadXAxisMin = DownloadChartValues.Last().DateTime.Ticks - TimeSpan.FromMinutes(30).Ticks;// TimeSpan.FromSeconds(6).Ticks;
                UploadXAxisMin = UploadChartValues.Last().DateTime.Ticks - TimeSpan.FromMinutes(30).Ticks;//TimeSpan.FromSeconds(6).Ticks;

                IsRefreshable = false;
            }
        }

        private void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private double _downloadXAxisMax;
        private double _downloadXAxisMin;
        private double _uploadXAxisMax;
        private double _uploadXAxisMin;

        private double _yAxisMax;
        private double _xAxisStep;
        private double? _ySeparatorStep;

        public ChartValues<MeasureModel> DownloadChartValues { get; set; }
        public ChartValues<MeasureModel> UploadChartValues { get; set; }

        public Func<double, string> DateTimeFormatter { get; set; }
        public Func<double, string> SizeFormatter { get; set; }

        public double XAxisStep {
            get
            {
                return _xAxisStep;
            }

            set
            {
                _xAxisStep = value;
                CallPropertyChanged("XAxisStep");
            }
        }

        public double DownloadXAxisMax
        {
            get { return _downloadXAxisMax; }
            set
            {
                _downloadXAxisMax = value;
                CallPropertyChanged("DownloadXAxisMax");
            }
        }
        public double DownloadXAxisMin
        {
            get { return _downloadXAxisMin; }
            set
            {
                _downloadXAxisMin = value;
                CallPropertyChanged("DownloadXAxisMin");
            }
        }
        public double UploadXAxisMax
        {
            get { return _downloadXAxisMax; }
            set
            {
                _uploadXAxisMax = value;
                CallPropertyChanged("UploadXAxisMax");
            }
        }
        public double UploadXAxisMin
        {
            get { return _uploadXAxisMin; }
            set
            {
                _uploadXAxisMin = value;
                CallPropertyChanged("UploadXAxisMin");
            }
        }

        public double YAxisMax
        {
            get
            {
                return _yAxisMax;
            }

            set
            {
                _yAxisMax = value;
                CallPropertyChanged("YAxisMax");
            }
        }

        public double? YSeparatorStep
        {
            get
            {
                return _ySeparatorStep;
            }

            set
            {
                _ySeparatorStep = value;
                CallPropertyChanged("YSeparatorStep");
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            DownloadXAxisMax = now.Ticks + TimeSpan.FromSeconds(0.1).Ticks; // lets force the axis to be 100ms ahead
            //DownloadXAxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; //we only care about the last 8 seconds

            UploadXAxisMax = now.Ticks + TimeSpan.FromSeconds(0.1).Ticks; // lets force the axis to be 100ms ahead
            //UploadXAxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; //we only care about the last 8 seconds

            if (YAxisMax < DownloadRate)
            {
                YAxisMax = DownloadRate + (DownloadRate/10);
            }

            if (YAxisMax < 500000)
            {
                YSeparatorStep = null;
            } else
            {
                YSeparatorStep = 500000;
            }
        }

        private void RefreshTimer(object sender, EventArgs e)
        {
            IsRefreshable = true;
        }
    }

    public class MeasureModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

}
