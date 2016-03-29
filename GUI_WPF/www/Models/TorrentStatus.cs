using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tsunami.Gui.Wpf.www.Models
{
    public class TorrentStatus
    {
        [JsonIgnore]
        public int active_time { get; set; }

        [JsonIgnore]
        public DateTime added_time { get; set; }

        [JsonIgnore]
        public long all_time_download { get; set; }

        [JsonIgnore]
        public long all_time_upload { get; set; }

        [JsonIgnore]
        public TimeSpan announce_interval { get; set; }

        [JsonIgnore]
        public bool auto_managed { get; set; }

        [JsonIgnore]
        public int block_size { get; set; }

        [JsonIgnore]
        public DateTime completed_time { get; set; }

        [JsonIgnore]
        public int connections_limit { get; set; }

        [JsonIgnore]
        public int connect_candidates { get; set; }

        [JsonIgnore]
        public string current_tracker { get; set; }

        [JsonIgnore]
        public float distributed_copies { get; set; }

        [JsonIgnore]
        public int distributed_fraction { get; set; }

        [JsonIgnore]
        public int distributed_full_copies { get; set; }

        [JsonIgnore]
        public int download_payload_rate { get; set; }

        [JsonIgnore]
        public int download_rate { get; set; }

        [JsonIgnore]
        public int down_bandwidth_queue { get; set; }

        [JsonIgnore]
        public string error { get; set; }

        [JsonIgnore]
        public int finished_time { get; set; }

        [JsonIgnore]
        public bool has_incoming { get; set; }

        [JsonIgnore]
        public bool has_metadata { get; set; }

        [JsonIgnore]
        public bool ip_filter_applies { get; set; }

        [JsonIgnore]
        public bool is_finished { get; set; }

        [JsonIgnore]
        public bool is_seeding { get; set; }

        [JsonIgnore]
        public int last_scrape { get; set; }

        [JsonIgnore]
        public DateTime last_seen_complete { get; set; }

        [JsonIgnore]
        public int list_peers { get; set; }

        [JsonIgnore]
        public int list_seeds { get; set; }

        [JsonIgnore]
        public bool moving_storage { get; set; }


        public string name { get; set; }

        [JsonIgnore]
        public bool need_save_resume { get; set; }

        [JsonIgnore]
        public TimeSpan next_announce { get; set; }

        [JsonIgnore]
        public int num_complete { get; set; }

        [JsonIgnore]
        public int num_connections { get; set; }

        [JsonIgnore]
        public int num_incomplete { get; set; }

        [JsonIgnore]
        public int num_peers { get; set; }

        [JsonIgnore]
        public int num_pieces { get; set; }

        [JsonIgnore]
        public int num_seeds { get; set; }

        [JsonIgnore]
        public int num_uploads { get; set; }

        [JsonIgnore]
        public bool paused { get; set; }

        [JsonIgnore]
        public int priority { get; set; }


        public float progress { get; set; }

        [JsonIgnore]
        public int progress_ppm { get; set; }

        [JsonIgnore]
        public int queue_position { get; set; }

        [JsonIgnore]
        public string save_path { get; set; }

        [JsonIgnore]
        public int seeding_time { get; set; }

        [JsonIgnore]
        public bool seed_mode { get; set; }

        [JsonIgnore]
        public int seed_rank { get; set; }

        [JsonIgnore]
        public bool sequential_download { get; set; }

        [JsonIgnore]
        public bool share_mode { get; set; }

        [JsonIgnore]
        public bool super_seeding { get; set; }

        [JsonIgnore]
        public int time_since_download { get; set; }

        [JsonIgnore]
        public int time_since_upload { get; set; }

        [JsonIgnore]
        public long total_done { get; set; }

        [JsonIgnore]
        public long total_download { get; set; }

        [JsonIgnore]
        public long total_failed_bytes { get; set; }

        [JsonIgnore]
        public long total_payload_download { get; set; }

        [JsonIgnore]
        public long total_payload_upload { get; set; }

        [JsonIgnore]
        public long total_reduntant_bytes { get; set; }

        [JsonIgnore]
        public long total_upload { get; set; }

        [JsonIgnore]
        public long total_wanted { get; set; }

        [JsonIgnore]
        public long total_wanted_done { get; set; }

        [JsonIgnore]
        public int uploads_limit { get; set; }

        [JsonIgnore]
        public bool upload_mode { get; set; }

        [JsonIgnore]
        public int upload_payload_rate { get; set; }

        [JsonIgnore]
        public int upload_rate { get; set; }

        [JsonIgnore]
        public int up_bandwidth_queue { get; set; }
    }
}
