using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Tsunami.www
{
    public class SignalRHub : Hub
    {
        public void NotifyUpdateProgress(SessionManager.OnTorrentUpdatedEventArgs e)
        {
            Clients.All.notifyUpdateProgress(e);
        }

        public void NotifyTorrentAdded(string hash)
        {
            Clients.All.notifyTorrentAdded(hash);
        }

        public void NotifyNeedRefreshList()
        {
            Clients.All.refreshList();
        }

        public void NotifyError(Models.ErrorCode ec)
        {
            Clients.All.notifyError(ec);
        }

        public void NotifySessionStatistics(SessionManager.OnSessionStatisticsEventArgs ss)
        {
            Clients.All.notifySessionStatistics(ss);
        }
    }
}
