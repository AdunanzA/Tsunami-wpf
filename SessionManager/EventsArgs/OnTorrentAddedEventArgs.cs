using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.EventsArgs
{
    public class OnTorrentAddedEventArgs : EventArgs
    {
        public string Hash { get; set; }
        public int QueuePosition { get; set; }
        public string Name { get; set; }
        public float Progress { get; set; }
        public string Status { get; set; }
    }
}
