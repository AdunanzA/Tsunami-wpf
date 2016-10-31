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
        private bool _isSelected;

        public int QueuePosition { get { return _QueuePosition; } set { if (_QueuePosition != value) { _QueuePosition = value; CallPropertyChanged("QueuePosition"); } } }
        public string Name { get { return _Name; } set { if (_Name != value) { _Name = value; CallPropertyChanged("Name"); } } }
        public string Hash { get { return _Hash; } set { if (_Hash != value) { _Hash = value; CallPropertyChanged("Hash"); } } }
        public long TotalWanted { get { return _TotalWanted; } set { if (_TotalWanted != value) { _TotalWanted = value; CallPropertyChanged("TotalWanted_ByteSize"); } } }
        public long TotalDone { get { return _TotalDone; } set { if (_TotalDone != value) { _TotalDone = value; CallPropertyChanged("TotalDone_ByteSize"); CallPropertyChanged("Remaining"); CallPropertyChanged("Remaining_ByteSize"); } } }
        public string State { get { return _State; } set { if (_State != value) { _State = value; CallPropertyChanged("State"); CallPropertyChanged("IsPauseButtonVisible"); CallPropertyChanged("IsResumeButtonVisible"); } } }

        /* Floating point numbers should not be tested for equality
         * https://www.misra.org.uk/forum/viewtopic.php?t=294 */
        public float Progress {
            get { return _Progress; }
            set {
                double tolerance = 0.001;
                if ((value >= (_Progress - tolerance) && (value <= (_Progress + tolerance))))
                {
                    _Progress = value;
                    CallPropertyChanged("Progress_String");
                    CallPropertyChanged("Progress_Number");
                    CallPropertyChanged("Progress_Color");
                }
            }
        }
        public int Priority { get { return _Priority; } set { if (_Priority != value) { _Priority = value; CallPropertyChanged("Priority_String"); } } }
        public int DownloadRate { get { return _DownloadRate; } set { if (_DownloadRate != value) { _DownloadRate = value; CallPropertyChanged("DownloadRate_ByteSize"); } } }
        public int UploadRate { get { return _UploadRate; } set { if (_UploadRate != value) { _UploadRate = value; CallPropertyChanged("UploadRate_ByteSize"); } } }
        public int NumConnections { get { return _NumConnections; } set { if (_NumConnections != value) { _NumConnections = value; CallPropertyChanged("NumConnections"); } } }
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
        public string UploadRate_ByteSize
        {
            get
            {
                //return Utils.StrFormatByteSize(_UploadRate) + @"/s";
                return _UploadRate.Bytes().Per(TimeSpan.FromSeconds(1)).Humanize("0.00");
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
        public string Remaining
        {
            get
            {
                if (DownloadRate == 0) return null;
                //return TimeSpan.FromSeconds((TotalWanted - TotalDone) / DownloadRate).Humanize(3, maxUnit: Humanizer.Localisation.TimeUnit.Hour);
                return string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds((TotalWanted - TotalDone) / DownloadRate));
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
                return new SolidColorBrush(Colors.Red).Color;
            }
        }

        public Color ColorTo
        {
            get
            {
                //Tuple<MahApps.Metro.AppTheme, MahApps.Metro.Accent> appStyle = MahApps.Metro.ThemeManager.DetectAppStyle(System.Windows.Application.Current);
                //SolidColorBrush scb = (SolidColorBrush)appStyle.Item2.Resources["HighlightBrush"];
                //return scb.Color;
                return new SolidColorBrush(Colors.LimeGreen).Color;
            }
        }

        public TorrentItem()
        {
            Formatter = x => ((int)x).ToString() + "%";
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
            Formatter = x => ((int)x).ToString() + "%";
        }

        public void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

}
