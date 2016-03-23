﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Gui.Wpf.www.Models
{
    public class TorrentItem
    {
        public int download_limit { get; set; }
        public int[] file_priorities { get; set; }
        public int file_priority { get; set; }
        public long[] file_progress { get; set; }
        /*public PeerInfo[] get_peer_info { get; set; }*/
        public bool has_metadata { get; set; }
        public bool have_piece { get; set; }
        public string[] http_seeds { get; set; }
        /*public Sha1Hash info_hash { get; set; }*/
        public bool is_valid { get; set; }
        public int max_connections { get; set; }
        public int max_uploads { get; set; }
        public bool need_save_resume_data { get; set; }
        public int[] piece_availability { get; set; }
        public int[] piece_priorities { get; set; }
        public int piece_priority { get; set; }
        public int queue_position { get; set; }
        public bool resolve_countries { get; set; }
        public TorrentStatus status { get; set; }
        public TorrentInfo torrent_file { get; set; }
        /*public AnnounceEntry[] trackers { get; set; }*/
        public int upload_limit { get; set; }
        public string[] url_seeds { get; set; }

    }
}