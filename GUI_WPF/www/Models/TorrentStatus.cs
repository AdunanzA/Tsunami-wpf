using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Gui.Wpf.www.Models
{
    public class TorrentStatus
    {
        public int active_time { get; set; }
        public DateTime added_time { get; set; }
        public long all_time_download { get; set; }
        public long all_time_upload { get; set; }
        public TimeSpan announce_interval { get; set; }
        public bool auto_managed { get; set; }
        public int block_size { get; set; }
        public DateTime completed_time { get; set; }
        public int connections_limit { get; set; }
        public int connect_candidates { get; set; }
        public string current_tracker { get; set; }
        public float distributed_copies { get; set; }
        public int distributed_fraction { get; set; }
        public int distributed_full_copies { get; set; }
        public int download_payload_rate { get; set; }
        public int download_rate { get; set; }
        public int down_bandwidth_queue { get; set; }
        public string error { get; set; }
        public int finished_time { get; set; }
        public bool has_incoming { get; set; }
        public bool has_metadata { get; set; }
        public bool ip_filter_applies { get; set; }
        public bool is_finished { get; set; }
        public bool is_seeding { get; set; }
        public int last_scrape { get; set; }
        public DateTime last_seen_complete { get; set; }
        public int list_peers { get; set; }
        public int list_seeds { get; set; }
        public bool moving_storage { get; set; }
        public string name { get; set; }
        public bool need_save_resume { get; set; }
        public TimeSpan next_announce { get; set; }
        public int num_complete { get; set; }
        public int num_connections { get; set; }
        public int num_incomplete { get; set; }
        public int num_peers { get; set; }
        public int num_pieces { get; set; }
        public int num_seeds { get; set; }
        public int num_uploads { get; set; }
        public bool paused { get; set; }
        public int priority { get; set; }
        public float progress { get; set; }
        public int progress_ppm { get; set; }
        public int queue_position { get; set; }
        public string save_path { get; set; }
        public int seeding_time { get; set; }
        public bool seed_mode { get; set; }
        public int seed_rank { get; set; }
        public bool sequential_download { get; set; }
        public bool share_mode { get; set; }
        public bool super_seeding { get; set; }
        public int time_since_download { get; set; }
        public int time_since_upload { get; set; }
        public long total_done { get; set; }
        public long total_download { get; set; }
        public long total_failed_bytes { get; set; }
        public long total_payload_download { get; set; }
        public long total_payload_upload { get; set; }
        public long total_reduntant_bytes { get; set; }
        public long total_upload { get; set; }
        public long total_wanted { get; set; }
        public long total_wanted_done { get; set; }
        public int uploads_limit { get; set; }
        public bool upload_mode { get; set; }
        public int upload_payload_rate { get; set; }
        public int upload_rate { get; set; }
        public int up_bandwidth_queue { get; set; }
    }
}
