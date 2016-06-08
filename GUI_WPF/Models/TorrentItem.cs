using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tsunami.Gui.Wpf
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
        
        public int QueuePosition { get { return _QueuePosition; } set { if (_QueuePosition != value) { _QueuePosition = value; CallPropertyChanged("QueuePosition"); } } }
        public string Name { get { return _Name; } set { if (_Name != value) { _Name = value; CallPropertyChanged("Name"); } } }
        public string Hash { get { return _Hash; } set { if (_Hash != value) { _Hash = value; CallPropertyChanged("Hash"); } } }
        public long TotalWanted { get { return _TotalWanted; } set { if (_TotalWanted != value) { _TotalWanted = value; CallPropertyChanged("TotalWanted_ByteSize"); } } }
        public long TotalDone { get { return _TotalDone; } set { if (_TotalDone != value) { _TotalDone = value; CallPropertyChanged("TotalDone_ByteSize"); } } }
        public string State { get { return _State; } set { if (_State != value) { _State = value; CallPropertyChanged("State"); } } }
        public float Progress { get { return _Progress; } set { if (_Progress != value) { _Progress = value; CallPropertyChanged("Progress_String"); CallPropertyChanged("Progress_Number"); CallPropertyChanged("Progress_Color"); } } }
        public int Priority { get { return _Priority; } set { if (_Priority != value) { _Priority = value; CallPropertyChanged("Priority_String"); } } }
        public int DownloadRate { get { return _DownloadRate; } set { if (_DownloadRate != value) { _DownloadRate = value; CallPropertyChanged("DownloadRate_ByteSize"); } } }
        public int UploadRate { get { return _UploadRate; } set { if (_UploadRate != value) { _UploadRate = value; CallPropertyChanged("UploadRate_ByteSize"); } } }

        public string Progress_String
        {
            get
            {
                return (Progress * 100).ToString("0.##") + "%";
            }
        }
        public float Progress_Number
        {
            get
            {
                return (Progress * 100);
            }
        }
        public string TotalWanted_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_TotalWanted);
            }
        }
        public string TotalDone_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_TotalDone);
            }
        }
        public string DownloadRate_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_DownloadRate) + @"/s";
            }
        }
        public string UploadRate_ByteSize
        {
            get
            {
                return Utils.StrFormatByteSize(_UploadRate) + @"/s";
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
        public Brush Progress_Color
        {
            get
            {
                Color cl = new Color();
                cl.A = 255;
                cl.R = (byte)((255 * (100 - Progress_Number)) / 100);
                cl.G = (byte)((255 * Progress_Number) / 100);
                cl.B = 0;

                return new SolidColorBrush(cl);
            }
        }

        public TorrentItem()
        {

        }

        public TorrentItem(int queue_position, string name, string hash, long total_wanted, long total_done, string state, float progress, int priority, int down_rate, int up_rate)
        {
            QueuePosition = queue_position;
            Name = name;
            Hash = hash;
            TotalWanted = total_wanted;
            TotalDone = total_done;
            State = state;
            Progress = progress;
            Priority = priority;
            DownloadRate = down_rate;
            UploadRate = up_rate;
        }

        private void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

}
