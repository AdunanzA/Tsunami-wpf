using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tsunami.Gui.Wpf.www.Models
{
    public class TorrentInfo
    {
        [JsonIgnore]
        public string comment { get; set; }

        [JsonIgnore]
        public ValueType creation_date { get; set; }

        [JsonIgnore]
        public string creator { get; set; }
        /*public Sha1Hash info_hash { get; set; }*/

        [JsonIgnore]
        public bool is_i2p { get; set; }

        [JsonIgnore]
        public bool is_merkle_torrent { get; set; }

        [JsonIgnore]
        public bool is_valid { get; set; }

        [JsonIgnore]
        public byte[] metadata { get; set; }

        [JsonIgnore]
        public int metadata_size { get; set; }

        [JsonIgnore]
        public string name { get; set; }

        [JsonIgnore]
        public int num_files { get; set; }

        [JsonIgnore]
        public int num_pieces { get; set; }

        [JsonIgnore]
        public int piece_length { get; set; }

        [JsonIgnore]
        public int piece_size { get; set; }

        [JsonIgnore]
        public bool priv { get; set; }

        [JsonIgnore]
        public string ssl_cert { get; set; }

        [JsonIgnore]
        public long total_size { get; set; }

    }
}
