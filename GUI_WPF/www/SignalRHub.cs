using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Tsunami.Gui.Wpf.www
{
    public class SignalRHub : Hub
    {
        public void NotifyUpdateProgress(string hash, int queuePosition, string torrentName, float progress, string status)
        {
            Clients.All.notifyUpdateProgress(hash, queuePosition, torrentName, progress, status);
        }

        public void NotifyTorrentAdded(string hash)
        {
            Clients.All.notifyTorrentAdded(hash);
        }

        public void NotifyNeedRefreshList()
        {
            Clients.All.refreshList();
        }
    }
}
