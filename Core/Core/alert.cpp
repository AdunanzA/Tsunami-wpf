#include "alert.h"

#include "alert_types.h"
#include "interop.h"

#define ALERT_CASE(alert_name) \
    case libtorrent::alert_name::alert_type: \
    { \
        libtorrent::alert_name* tmp = libtorrent::alert_cast<libtorrent::alert_name>(a); \
        return gcnew alert_name(tmp); \
    }

using namespace Tsunami::Core;

Alert::Alert(libtorrent::alert* al)
{
    alert_ = al;
}

Alert::~Alert()
{
    delete alert_;
}

Alert^ Alert::create(std::auto_ptr<libtorrent::alert> al)
{
    libtorrent::alert* a = al->clone().release();

    switch (a->type())
    {
        ALERT_CASE(torrent_added_alert);
        ALERT_CASE(torrent_removed_alert);
		ALERT_CASE(torrent_finished_alert);
		ALERT_CASE(state_update_alert);
		ALERT_CASE(read_piece_alert);
		ALERT_CASE(file_completed_alert);
		ALERT_CASE(file_renamed_alert);
		ALERT_CASE(file_rename_failed_alert);
		ALERT_CASE(performance_alert);
		ALERT_CASE(state_changed_alert);
		ALERT_CASE(tracker_error_alert);
		ALERT_CASE(tracker_warning_alert);
		ALERT_CASE(scrape_reply_alert);
		ALERT_CASE(scrape_failed_alert);
		ALERT_CASE(tracker_reply_alert);
		ALERT_CASE(dht_reply_alert);
		ALERT_CASE(tracker_announce_alert);
		ALERT_CASE(hash_failed_alert);
		ALERT_CASE(peer_ban_alert);
		ALERT_CASE(peer_unsnubbed_alert);
		ALERT_CASE(peer_snubbed_alert);
		ALERT_CASE(peer_error_alert);
		ALERT_CASE(peer_connect_alert);
		ALERT_CASE(peer_disconnected_alert);
		ALERT_CASE(invalid_request_alert);
		ALERT_CASE(piece_finished_alert);
		ALERT_CASE(request_dropped_alert);
		ALERT_CASE(block_timeout_alert);
		ALERT_CASE(block_finished_alert);
		ALERT_CASE(block_downloading_alert);
		ALERT_CASE(unwanted_block_alert);
		ALERT_CASE(storage_moved_alert);
		ALERT_CASE(storage_moved_failed_alert);
		ALERT_CASE(torrent_deleted_alert);
		ALERT_CASE(torrent_delete_failed_alert);
		ALERT_CASE(save_resume_data_alert);
		ALERT_CASE(save_resume_data_failed_alert);
		ALERT_CASE(torrent_paused_alert);
		ALERT_CASE(torrent_resumed_alert);
		ALERT_CASE(torrent_checked_alert);
		ALERT_CASE(url_seed_alert);
		ALERT_CASE(file_error_alert);
		ALERT_CASE(metadata_failed_alert);
		ALERT_CASE(metadata_received_alert);
		ALERT_CASE(udp_error_alert);
		ALERT_CASE(external_ip_alert);
		ALERT_CASE(listen_failed_alert);
		ALERT_CASE(listen_succeeded_alert);
		ALERT_CASE(portmap_error_alert);
		ALERT_CASE(portmap_alert);
		ALERT_CASE(portmap_log_alert);
		ALERT_CASE(fastresume_rejected_alert);
		ALERT_CASE(peer_blocked_alert);
		ALERT_CASE(dht_announce_alert);
		ALERT_CASE(dht_get_peers_alert);
		ALERT_CASE(stats_alert);
		ALERT_CASE(cache_flushed_alert);
		ALERT_CASE(anonymous_mode_alert);
		ALERT_CASE(lsd_peer_alert);
		ALERT_CASE(trackerid_alert);
		ALERT_CASE(dht_bootstrap_alert);
		ALERT_CASE(rss_alert);
		ALERT_CASE(torrent_error_alert);
		ALERT_CASE(torrent_need_cert_alert);
		ALERT_CASE(incoming_connection_alert);
		ALERT_CASE(add_torrent_alert);
		ALERT_CASE(torrent_update_alert);
		ALERT_CASE(rss_item_alert);
		ALERT_CASE(dht_error_alert);
		ALERT_CASE(dht_immutable_item_alert);
		ALERT_CASE(dht_mutable_item_alert);
		ALERT_CASE(dht_put_alert);
		ALERT_CASE(i2p_alert);
    default:
        return gcnew Alert(a);
    }

    throw gcnew System::NotImplementedException();
}




System::DateTime Alert::timestamp()
{
	return System::DateTime::Now;
}

int Alert::type()
{
    return alert_->type();
}

System::String^ Alert::what()
{
    return interop::from_std_string(alert_->what());
}

System::String^ Alert::message()
{
    return interop::from_std_string(alert_->message());
}

int Alert::category()
{
    return alert_->category();
}

bool Alert::discardable()
{
    return alert_->discardable();
}
