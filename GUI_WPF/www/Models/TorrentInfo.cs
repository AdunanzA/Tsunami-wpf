using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Gui.Wpf.www.Models
{
    public class TorrentInfo
    {
        public string comment { get; set; }
        public ValueType creation_date { get; set; }
        public string creator { get; set; }
        /*public Sha1Hash info_hash { get; set; }*/
        public bool is_i2p { get; set; }
        public bool is_merkle_torrent { get; set; }
        public bool is_valid { get; set; }
        public byte[] metadata { get; set; }
        public int metadata_size { get; set; }
        public string name { get; set; }
        public int num_files { get; set; }
        public int num_pieces { get; set; }
        public int piece_length { get; set; }
        public int piece_size { get; set; }
        public bool priv { get; set; }
        public string ssl_cert { get; set; }
        public long total_size { get; set; }

    }
}
