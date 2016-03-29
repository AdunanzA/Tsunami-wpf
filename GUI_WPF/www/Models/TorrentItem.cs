using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tsunami.Gui.Wpf.www.Models
{
    public class TorrentItem
    {
        [JsonIgnore]
        public int download_limit { get; set; }

        [JsonIgnore]
        public int[] file_priorities { get; set; }

        [JsonIgnore]
        public int file_priority { get; set; }

        [JsonIgnore]
        public long[] file_progress { get; set; }
        /*public PeerInfo[] get_peer_info { get; set; }*/

        [JsonIgnore]
        public bool has_metadata { get; set; }

        [JsonIgnore]
        public bool have_piece { get; set; }

        [JsonIgnore]
        public string[] http_seeds { get; set; }
        /*public Sha1Hash info_hash { get; set; }*/

        [JsonIgnore]
        public bool is_valid { get; set; }

        [JsonIgnore]
        public int max_connections { get; set; }

        [JsonIgnore]
        public int max_uploads { get; set; }

        [JsonIgnore]
        public bool need_save_resume_data { get; set; }

        [JsonIgnore]
        public int[] piece_availability { get; set; }

        [JsonIgnore]
        public int[] piece_priorities { get; set; }

        [JsonIgnore]
        public int piece_priority { get; set; }


        public int queue_position { get; set; }

        [JsonIgnore]
        public bool resolve_countries { get; set; }

        
        public TorrentStatus status { get; set; }

        
        public TorrentInfo torrent_file { get; set; }
        /*public AnnounceEntry[] trackers { get; set; }*/

        [JsonIgnore]
        public int upload_limit { get; set; }

        [JsonIgnore]
        public string[] url_seeds { get; set; }

    }
}
