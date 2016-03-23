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
        public void NotifyUpdateProgress(string torrentName, float progress)
        {
            Clients.All.notifyUpdateProgress(torrentName, progress);
        }
    }
}
