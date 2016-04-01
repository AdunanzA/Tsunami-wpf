#pragma once

#include <libtorrent/torrent_handle.hpp>
#include <libtorrent/torrent_status.hpp>
#include <libtorrent/torrent_info.hpp>

#include "enums.h"


namespace Tsunami
{
	namespace Core
	{
		ref class TorrentHandle;
		ref class TorrentInfo;
		ref class Sha1Hash;
		ref class BitField;

		public ref class TorrentStatus
		{
		internal:
			TorrentStatus(libtorrent::torrent_status& status);

		public:
			~TorrentStatus();

			

			enum class state_t
			{
#ifndef TORRENT_NO_DEPRECATE
				// The torrent is in the queue for being checked. But there
				// currently is another torrent that are being checked.
				// This torrent will wait for its turn.
				queued_for_checking,
#else
				// internal
				unused_enum_for_backwards_compatibility,
#endif

				// The torrent has not started its download yet, and is
				// currently checking existing files.
				checking_files,

				// The torrent is trying to download metadata from peers.
				// This assumes the metadata_transfer extension is in use.
				downloading_metadata,

				// The torrent is being downloaded. This is the state
				// most torrents will be in most of the time. The progress
				// meter will tell how much of the files that has been
				// downloaded.
				downloading,

				// In this state the torrent has finished downloading but
				// still doesn't have the entire torrent. i.e. some pieces
				// are filtered and won't get downloaded.
				finished,

				// In this state the torrent has finished downloading and
				// is a pure seeder.
				seeding,

				// If the torrent was started in full allocation mode, this
				// indicates that the (disk) storage for the torrent is
				// allocated.
				allocating,

				// The torrent is currently checking the fastresume data and
				// comparing it to the files on disk. This is typically
				// completed in a fraction of a second, but if you add a
				// large number of torrents at once, they will queue up.
				checking_resume_data
			};

			
			TorrentHandle^ handle();
			TorrentInfo ^ torrent_file();
			property System::String^ error { System::String^ get(); }
			property System::String^ save_path { System::String^ get(); }
			property System::String^ name { System::String^ get(); }
			property System::TimeSpan next_announce { System::TimeSpan get(); }
			property System::TimeSpan announce_interval { System::TimeSpan get(); }
			property System::String^ current_tracker { System::String^ get(); }
			property long long total_download { long long get(); }
			property long long total_upload { long long get(); }
			property long long total_payload_download { long long get(); }
			property long long total_payload_upload { long long get(); }
			property long long total_failed_bytes { long long get(); }
			property long long total_reduntant_bytes { long long get(); }
			property BitField ^ pieces {BitField ^ get(); }
			property BitField ^ verified_pieces {BitField ^ get(); }
			property long long total_done { long long get(); }
			property long long total_wanted_done { long long get(); }
			property long long total_wanted { long long get(); }
			property long long all_time_upload { long long get(); }
			property long long all_time_download { long long get(); }
			property System::DateTime added_time { System::DateTime get(); }
			property System::DateTime completed_time { System::DateTime get(); }
			property System::DateTime last_seen_complete { System::DateTime get(); }
			property storage_mode_t ^ storage_mode { storage_mode_t ^ get(); }
			property float progress { float get(); }
			property int progress_ppm { int get(); }
			property int queue_position { int get(); }
			property int download_rate { int get(); }
			property int upload_rate { int get(); }
			property int download_payload_rate { int get(); }
			property int upload_payload_rate { int get(); }
			property int num_seeds { int get(); }
			property int num_peers { int get(); }
			property int num_complete { int get(); }
			property int num_incomplete { int get(); }
			property int list_seeds { int get(); }
			property int list_peers { int get(); }
			property int connect_candidates { int get(); }
			property int num_pieces { int get(); }
			property int distributed_full_copies { int get(); }
			property int distributed_fraction { int get(); }
			property float distributed_copies { float get(); }
			property int block_size { int get(); }
			property int num_uploads { int get(); }
			property int num_connections { int get(); }
			property int uploads_limit { int get(); }
			property int connections_limit { int get(); }
			property int up_bandwidth_queue { int get(); }
			property int down_bandwidth_queue { int get(); }
			property int time_since_upload { int get(); }
			property int time_since_download { int get(); }
			property int active_time { int get(); }
			property int finished_time { int get(); }
			property int seeding_time { int get(); }
			property int seed_rank { int get(); }
			property int last_scrape { int get(); }
			//property int sparse_regions { int get(); }
			property int priority { int get(); }
			property state_t^ state { state_t^ get(); }
			property bool need_save_resume { bool get(); }
			property bool ip_filter_applies { bool get(); }
			property bool upload_mode { bool get(); }
			property bool share_mode { bool get(); }
			property bool super_seeding { bool get(); }
			property bool paused { bool get(); }
			property bool auto_managed { bool get(); }
			property bool sequential_download { bool get(); }
			property bool is_seeding { bool get(); }
			property bool is_finished { bool get(); }
			property bool has_metadata { bool get(); }
			property bool has_incoming { bool get(); }
			property bool seed_mode { bool get(); }
			property bool moving_storage { bool get(); }
			property Sha1Hash ^ info_hash { Sha1Hash ^ get(); }
			

		private:
			libtorrent::torrent_status* status_;
		};
	}
}
