#include "torrent_status.h"

#include "torrent_handle.h"
#include "torrent_info.h"
#include "interop.h"
#include "sha1_hash.h"
#include "bitfield.h"

using namespace Tsunami::Core;

TorrentStatus::TorrentStatus(libtorrent::torrent_status& status)
{
    status_ = new libtorrent::torrent_status(status);
}

TorrentStatus::~TorrentStatus()
{
    delete status_;
}

TorrentHandle ^ TorrentStatus::handle()
{
	return gcnew TorrentHandle(status_->handle);
}

TorrentInfo ^ TorrentStatus::torrent_file()
{
	return gcnew TorrentInfo(*(status_->torrent_file.lock().get()));
}

System::String^ TorrentStatus::error::get()
{
    return interop::from_std_string(status_->error);
}

System::String^ TorrentStatus::save_path::get()
{
    return interop::from_std_string(status_->save_path);
}

System::String^ TorrentStatus::name::get()
{
    return interop::from_std_string(status_->name);
}

System::TimeSpan TorrentStatus::next_announce::get()
{
	using std::chrono::duration_cast;
	using std::chrono::seconds;
	return System::TimeSpan(0, 0, (int)duration_cast<seconds>(status_->next_announce).count());
}

System::TimeSpan TorrentStatus::announce_interval::get()
{
	using std::chrono::duration_cast;
	using std::chrono::seconds;
    return System::TimeSpan(0, 0, (int)duration_cast<seconds>(status_->announce_interval).count());
}

System::String^ TorrentStatus::current_tracker::get()
{
    return interop::from_std_string(status_->current_tracker);
}

long long TorrentStatus::total_download::get()
{
    return status_->total_download;
}

long long TorrentStatus::total_upload::get()
{
    return status_->total_upload;
}

long long TorrentStatus::total_payload_download::get()
{
    return status_->total_payload_download;
}

long long TorrentStatus::total_payload_upload::get()
{
    return status_->total_payload_upload;
}

long long TorrentStatus::total_failed_bytes::get()
{
    return status_->total_failed_bytes;
}

long long TorrentStatus::total_reduntant_bytes::get()
{
    return status_->total_redundant_bytes;
}

long long TorrentStatus::total_done::get()
{
    return status_->total_done;
}

long long TorrentStatus::total_wanted_done::get()
{
    return status_->total_wanted_done;
}

long long TorrentStatus::total_wanted::get()
{
    return status_->total_wanted;
}

long long TorrentStatus::all_time_upload::get()
{
    return status_->all_time_upload;
}

long long TorrentStatus::all_time_download::get()
{
    return status_->all_time_download;
}

System::DateTime TorrentStatus::added_time::get()
{
    return System::DateTime(1970, 1, 1, 0, 0, 0, 0, System::DateTimeKind::Utc).AddSeconds((double)status_->added_time).ToLocalTime();
}

System::DateTime TorrentStatus::completed_time::get()
{
    return System::DateTime(1970, 1, 1, 0, 0, 0, 0, System::DateTimeKind::Utc).AddSeconds((double)status_->completed_time).ToLocalTime();
}

System::DateTime TorrentStatus::last_seen_complete::get()
{
    return System::DateTime(1970, 1, 1, 0, 0, 0, 0, System::DateTimeKind::Utc).AddSeconds((double)status_->last_seen_complete).ToLocalTime();
}

float TorrentStatus::progress::get()
{
    return status_->progress;
}

int TorrentStatus::progress_ppm::get()
{
    return status_->progress_ppm;
}

int TorrentStatus::queue_position::get()
{
    return status_->queue_position;
}

int TorrentStatus::download_rate::get()
{
    return status_->download_rate;
}

int TorrentStatus::upload_rate::get()
{
    return status_->upload_rate;
}

int TorrentStatus::download_payload_rate::get()
{
    return status_->download_payload_rate;
}

int TorrentStatus::upload_payload_rate::get()
{
    return status_->upload_payload_rate;
}

int TorrentStatus::num_seeds::get()
{
    return status_->num_seeds;
}

int TorrentStatus::num_peers::get()
{
    return status_->num_peers;
}

int TorrentStatus::num_complete::get()
{
    return status_->num_complete;
}

int TorrentStatus::num_incomplete::get()
{
    return status_->num_incomplete;
}

int TorrentStatus::list_seeds::get()
{
    return status_->list_seeds;
}

int TorrentStatus::list_peers::get()
{
    return status_->list_peers;
}

int TorrentStatus::connect_candidates::get()
{
    return status_->connect_candidates;
}

int TorrentStatus::num_pieces::get()
{
    return status_->num_pieces;
}

int TorrentStatus::distributed_full_copies::get()
{
    return status_->distributed_full_copies;
}

int TorrentStatus::distributed_fraction::get()
{
    return status_->distributed_fraction;
}

float TorrentStatus::distributed_copies::get()
{
    return status_->distributed_copies;
}

int TorrentStatus::block_size::get()
{
    return status_->block_size;
}

int TorrentStatus::num_uploads::get()
{
    return status_->num_uploads;
}

int TorrentStatus::num_connections::get()
{
    return status_->num_connections;
}

int TorrentStatus::uploads_limit::get()
{
    return status_->uploads_limit;
}

int TorrentStatus::connections_limit::get()
{
    return status_->connections_limit;
}

int TorrentStatus::up_bandwidth_queue::get()
{
    return status_->up_bandwidth_queue;
}

int TorrentStatus::down_bandwidth_queue::get()
{
    return status_->down_bandwidth_queue;
}

int TorrentStatus::time_since_upload::get()
{
    return status_->time_since_upload;
}

int TorrentStatus::time_since_download::get()
{
    return status_->time_since_download;
}

int TorrentStatus::active_time::get()
{
    return status_->active_time;
}

int TorrentStatus::finished_time::get()
{
    return status_->finished_time;
}

int TorrentStatus::seeding_time::get()
{
    return status_->seeding_time;
}

int TorrentStatus::seed_rank::get()
{
    return status_->seed_rank;
}

int TorrentStatus::last_scrape::get()
{
    return status_->last_scrape;
}

/*int torrent_status::sparse_regions::get()
{
    return status_->sparse_regions;
}*/

int TorrentStatus::priority::get()
{
    return status_->priority;
}

bool TorrentStatus::need_save_resume::get()
{
    return status_->need_save_resume;
}

bool TorrentStatus::ip_filter_applies::get()
{
    return status_->ip_filter_applies;
}

bool TorrentStatus::upload_mode::get()
{
    return status_->upload_mode;
}

bool TorrentStatus::share_mode::get()
{
    return status_->share_mode;
}

bool TorrentStatus::super_seeding::get()
{
    return status_->super_seeding;
}

bool TorrentStatus::paused::get()
{
    return status_->paused;
}

bool TorrentStatus::auto_managed::get()
{
    return status_->auto_managed;
}

bool TorrentStatus::sequential_download::get()
{
    return status_->sequential_download;
}

bool TorrentStatus::is_seeding::get()
{
    return status_->is_seeding;
}

bool TorrentStatus::is_finished::get()
{
    return status_->is_finished;
}

bool TorrentStatus::has_metadata::get()
{
    return status_->has_metadata;
}

bool TorrentStatus::has_incoming::get()
{
    return status_->has_incoming;
}

bool TorrentStatus::seed_mode::get()
{
    return status_->seed_mode;
}

bool TorrentStatus::moving_storage::get()
{
    return status_->moving_storage;
}

TorrentStatus::state_t TorrentStatus::state::get()
{
	return static_cast<TorrentStatus::state_t>(status_->state);
}

Sha1Hash ^ TorrentStatus::info_hash::get()
{
	return gcnew Sha1Hash(status_->info_hash);
}

BitField ^ TorrentStatus::pieces::get()
{
	return gcnew BitField(status_->pieces);
}

BitField ^ TorrentStatus::verified_pieces::get()
{
	return gcnew BitField(status_->verified_pieces);
}

storage_mode_t  TorrentStatus::storage_mode::get()
{
	return static_cast<storage_mode_t>(status_->storage_mode);
}
