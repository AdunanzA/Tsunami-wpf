using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tsunami.Gui.Wpf
{
    public class TorrentStatusViewModel
    {
        private ObservableCollection<TorrentItem> _torrentList { get; set; }
        private SessionStatistics _sessionStatistics { get; set; }
        private Preferences _preferences { get; set; }


        private object lockObj = new object();

        public TorrentStatusViewModel()
        {
            _torrentList = new ObservableCollection<TorrentItem>();
            _sessionStatistics = new SessionStatistics();
            _preferences = new Preferences();


            SessionManager.Instance.TorrentUpdated += new EventHandler<EventsArgs.OnTorrentUpdatedEventArgs>(UpdateFromTsunamiCore);
            SessionManager.Instance.TorrentAdded += new EventHandler<EventsArgs.OnTorrentAddedEventArgs>(AddFromTsunamiCore);
            SessionManager.Instance.TorrentRemoved += new EventHandler<EventsArgs.OnTorrentRemovedEventArgs>(RemovedFromTsunamiCore);
            SessionManager.Instance.SessionStatisticsUpdate += new EventHandler<EventsArgs.OnSessionStatisticsEventArgs>(UpdateFromSessionStatistics);
        }

        public ObservableCollection<TorrentItem> TorrentList
        {
            get
            {
                return _torrentList;
            }
        }

        public SessionStatistics SessionStat
        {
            get
            {
                return _sessionStatistics;
            }
        }

        public Preferences UserPreferences
        {
            get
            {
                return _preferences;
            }
        }

        private void UpdateFromSessionStatistics(object sender, EventsArgs.OnSessionStatisticsEventArgs e)
        {
            _sessionStatistics.Update(e);
        }

        private void RemovedFromTsunamiCore(object sender, EventsArgs.OnTorrentRemovedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => {
                TorrentItem ob = _torrentList.FirstOrDefault(o => o.Hash == e.Hash);
                _torrentList.Remove(ob);
            });
        }

        private void AddFromTsunamiCore(object sender, EventsArgs.OnTorrentAddedEventArgs e)
        {
            TorrentItem ob = new TorrentItem(e.QueuePosition, e.Name, e.Hash, 0, 0, e.Status, e.Progress, 0, 0, 0);
            System.Windows.Application.Current.Dispatcher.Invoke(() => {
                _torrentList.Add(ob);
            });
        }

        private void UpdateFromTsunamiCore(object sender, EventsArgs.OnTorrentUpdatedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => {
                TorrentItem ob = _torrentList.FirstOrDefault(o => o.Hash == e.InfoHash);
                ob.Name = e.Name;
                ob.QueuePosition = e.QueuePosition;
                ob.TotalDone = e.TotalDone;
                ob.TotalWanted = e.TotalWanted;
                ob.State = e.State;
                ob.Progress = e.Progress;
                ob.Priority = e.Priority;
                ob.DownloadRate = e.DownloadRate;
                ob.UploadRate = e.UploadRate;
            });
        }
    }
}
