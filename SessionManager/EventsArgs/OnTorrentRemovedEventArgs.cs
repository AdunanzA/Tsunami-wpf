using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.EventsArgs
{
    public class OnTorrentRemovedEventArgs : EventArgs
    {
        public string Hash { get; set; }
    }
}
