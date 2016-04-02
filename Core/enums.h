#pragma once


namespace Tsunami
{
	namespace Core
	{
		[System::FlagsAttribute]
		public enum class AlertMask : System::UInt32
		{
			// Enables alerts that report an error. This includes:
			// 
			// * tracker errors
			// * tracker warnings
			// * file errors
			// * resume data failures
			// * web seed errors
			// * .torrent files errors
			// * listen socket errors
			// * port mapping errors
			error_notification = 0x1,

			// Enables alerts when peers send invalid requests, get banned or
			// snubbed.
			peer_notification = 0x2,

			// Enables alerts for port mapping events. For NAT-PMP and UPnP.
			port_mapping_notification = 0x4,

			// Enables alerts for events related to the storage. File errors and
			// synchronization events for moving the storage, renaming files etc.
			storage_notification = 0x8,

			// Enables all tracker events. Includes announcing to trackers,
			// receiving responses, warnings and errors.
			tracker_notification = 0x10,

			// Low level alerts for when peers are connected and disconnected.
			debug_notification = 0x20,

			// Enables alerts for when a torrent or the session changes state.
			status_notification = 0x40,

			// Alerts for when blocks are requested and completed. Also when
			// pieces are completed.
			progress_notification = 0x80,

			// Alerts when a peer is blocked by the ip blocker or port blocker.
			ip_block_notification = 0x100,

			// Alerts when some limit is reached that might limit the download
			// or upload rate.
			performance_warning = 0x200,

			// Alerts on events in the DHT node. For incoming searches or
			// bootstrapping being done etc.
			dht_notification = 0x400,

			// If you enable these alerts, you will receive a stats_alert
			// approximately once every second, for every active torrent.
			// These alerts contain all statistics counters for the interval since
			// the lasts stats alert.
			stats_notification = 0x800,

#ifndef TORRENT_NO_DEPRECATE
			// Alerts on RSS related events, like feeds being updated, feed error
			// conditions and successful RSS feed updates. Enabling this categoty
			// will make you receive rss_alert alerts.
			rss_notification = 0x1000,
#endif

			// Enables debug logging alerts. These are available unless libtorrent
			// was built with logging disabled (``TORRENT_DISABLE_LOGGING``). The
			// alerts being posted are log_alert and are session wide.
			session_log_notification = 0x2000,

			// Enables debug logging alerts for torrents. These are available
			// unless libtorrent was built with logging disabled
			// (``TORRENT_DISABLE_LOGGING``). The alerts being posted are
			// torrent_log_alert and are torrent wide debug events.
			torrent_log_notification = 0x4000,

			// Enables debug logging alerts for peers. These are available unless
			// libtorrent was built with logging disabled
			// (``TORRENT_DISABLE_LOGGING``). The alerts being posted are
			// peer_log_alert and low-level peer events and messages.
			peer_log_notification = 0x8000,

			// enables the incoming_request_alert.
			incoming_request_notification = 0x10000,

			// enables dht_log_alert, debug logging for the DHT
			dht_log_notification = 0x20000,

			// enable events from pure dht operations not related to torrents
			dht_operation_notification = 0x40000,

			// enables port mapping log events. This log is useful
			// for debugging the UPnP or NAT-PMP implementation
			port_mapping_log_notification = 0x80000,

			// enables verbose logging from the piece picker.
			picker_log_notification = 0x100000,

			// The full bitmask, representing all available categories.
			//
			// since the enum is signed, make sure this isn't
			// interpreted as -1. For instance, boost.python
			// does that and fails when assigning it to an
			// unsigned parameter.
			all_categories = 0x7fffffff
		};


		public enum class performance_warning_t
		{

			// This warning means that the number of bytes queued to be written to disk
			// exceeds the max disk byte queue setting (``settings_pack::max_queued_disk_bytes``).
			// This might restrict the download rate, by not queuing up enough write jobs
			// to the disk I/O thread. When this alert is posted, peer connections are
			// temporarily stopped from downloading, until the queued disk bytes have fallen
			// below the limit again. Unless your ``max_queued_disk_bytes`` setting is already
			// high, you might want to increase it to get better performance.
			outstanding_disk_buffer_limit_reached,

			// This is posted when libtorrent would like to send more requests to a peer,
			// but it's limited by ``settings_pack::max_out_request_queue``. The queue length
			// libtorrent is trying to achieve is determined by the download rate and the
			// assumed round-trip-time (``settings_pack::request_queue_time``). The assumed
			// round-trip-time is not limited to just the network RTT, but also the remote disk
			// access time and message handling time. It defaults to 3 seconds. The target number
			// of outstanding requests is set to fill the bandwidth-delay product (assumed RTT
			// times download rate divided by number of bytes per request). When this alert
			// is posted, there is a risk that the number of outstanding requests is too low
			// and limits the download rate. You might want to increase the ``max_out_request_queue``
			// setting.
			outstanding_request_limit_reached,

			// This warning is posted when the amount of TCP/IP overhead is greater than the
			// upload rate limit. When this happens, the TCP/IP overhead is caused by a much
			// faster download rate, triggering TCP ACK packets. These packets eat into the
			// rate limit specified to libtorrent. When the overhead traffic is greater than
			// the rate limit, libtorrent will not be able to send any actual payload, such
			// as piece requests. This means the download rate will suffer, and new requests
			// can be sent again. There will be an equilibrium where the download rate, on
			// average, is about 20 times the upload rate limit. If you want to maximize the
			// download rate, increase the upload rate limit above 5% of your download capacity.
			upload_limit_too_low,

			// This is the same warning as ``upload_limit_too_low`` but referring to the download
			// limit instead of upload. This suggests that your download rate limit is much lower
			// than your upload capacity. Your upload rate will suffer. To maximize upload rate,
			// make sure your download rate limit is above 5% of your upload capacity.
			download_limit_too_low,

			// We're stalled on the disk. We want to write to the socket, and we can write
			// but our send buffer is empty, waiting to be refilled from the disk.
			// This either means the disk is slower than the network connection
			// or that our send buffer watermark is too small, because we can
			// send it all before the disk gets back to us.
			// The number of bytes that we keep outstanding, requested from the disk, is calculated
			// as follows::
			// 
			//   min(512, max(upload_rate * send_buffer_watermark_factor / 100, send_buffer_watermark))
			// 
			// If you receive this alert, you might want to either increase your ``send_buffer_watermark``
			// or ``send_buffer_watermark_factor``.
			send_buffer_watermark_too_low,

			// If the half (or more) of all upload slots are set as optimistic unchoke slots, this
			// warning is issued. You probably want more regular (rate based) unchoke slots.
			too_many_optimistic_unchoke_slots,

			// If the disk write queue ever grows larger than half of the cache size, this warning
			// is posted. The disk write queue eats into the total disk cache and leaves very little
			// left for the actual cache. This causes the disk cache to oscillate in evicting large
			// portions of the cache before allowing peers to download any more, onto the disk write
			// queue. Either lower ``max_queued_disk_bytes`` or increase ``cache_size``.
			too_high_disk_queue_limit,

			aio_limit_reached,
			bittyrant_with_no_uplimit,

			// This is generated if outgoing peer connections are failing because of *address in use*
			// errors, indicating that ``settings_pack::outgoing_ports`` is set and is too small of
			// a range. Consider not using the ``outgoing_ports`` setting at all, or widen the range to
			// include more ports.
			too_few_outgoing_ports,

			too_few_file_descriptors,

			num_warnings
		};

		public enum class storage_mode_t
		{
			// All pieces will be written to their final position, all files will be
			// allocated in full when the torrent is first started. This is done with
			// ``fallocate()`` and similar calls. This mode minimizes fragmentation.
			storage_mode_allocate,

			// All pieces will be written to the place where they belong and sparse files
			// will be used. This is the recommended, and default mode.
			storage_mode_sparse,

			// internal
			internal_storage_mode_compact_deprecated
#ifndef TORRENT_NO_DEPRECATE
			, // comma here to avoid compiler warning
			storage_mode_compact = internal_storage_mode_compact_deprecated
#endif
		};

		[System::FlagsAttribute]
		public enum class ATPFlags : System::UInt32
		{
			// If ``flag_seed_mode`` is set, libtorrent will assume that all files
			// are present for this torrent and that they all match the hashes in
			// the torrent file. Each time a peer requests to download a block,
			// the piece is verified against the hash, unless it has been verified
			// already. If a hash fails, the torrent will automatically leave the
			// seed mode and recheck all the files. The use case for this mode is
			// if a torrent is created and seeded, or if the user already know
			// that the files are complete, this is a way to avoid the initial
			// file checks, and significantly reduce the startup time.
			// 
			// Setting ``flag_seed_mode`` on a torrent without metadata (a
			// .torrent file) is a no-op and will be ignored.
			// 
			// If resume data is passed in with this torrent, the seed mode saved
			// in there will override the seed mode you set here.
			flag_seed_mode = 0x001,

#ifndef TORRENT_NO_DEPRECATE
			// If ``flag_override_resume_data`` is set, flags set for this torrent
			// in this ``add_torrent_params`` object will take precedence over
			// whatever states are saved in the resume data. For instance, the
			// ``paused``, ``auto_managed``, ``sequential_download``, ``seed_mode``,
			// ``super_seeding``, ``max_uploads``, ``max_connections``,
			// ``upload_limit`` and ``download_limit`` are all affected by this
			// flag. The intention of this flag is to have any field in
			// add_torrent_params configuring the torrent override the corresponding
			// configuration from the resume file, with the one exception of save
			// resume data, which has its own flag (for historic reasons).
			// If this flag is set, but file_priorities is empty, file priorities
			// are still loaded from the resume data, if present.
			flag_override_resume_data = 0x002,
#endif

			// If ``flag_upload_mode`` is set, the torrent will be initialized in
			// upload-mode, which means it will not make any piece requests. This
			// state is typically entered on disk I/O errors, and if the torrent
			// is also auto managed, it will be taken out of this state
			// periodically. This mode can be used to avoid race conditions when
			// adjusting priorities of pieces before allowing the torrent to start
			// downloading.
			// 
			// If the torrent is auto-managed (``flag_auto_managed``), the torrent
			// will eventually be taken out of upload-mode, regardless of how it
			// got there. If it's important to manually control when the torrent
			// leaves upload mode, don't make it auto managed.
			flag_upload_mode = 0x004,

			// determines if the torrent should be added in *share mode* or not.
			// Share mode indicates that we are not interested in downloading the
			// torrent, but merely want to improve our share ratio (i.e. increase
			// it). A torrent started in share mode will do its best to never
			// download more than it uploads to the swarm. If the swarm does not
			// have enough demand for upload capacity, the torrent will not
			// download anything. This mode is intended to be safe to add any
			// number of torrents to, without manual screening, without the risk
			// of downloading more than is uploaded.
			// 
			// A torrent in share mode sets the priority to all pieces to 0,
			// except for the pieces that are downloaded, when pieces are decided
			// to be downloaded. This affects the progress bar, which might be set
			// to "100% finished" most of the time. Do not change file or piece
			// priorities for torrents in share mode, it will make it not work.
			// 
			// The share mode has one setting, the share ratio target, see
			// ``session_settings::share_mode_target`` for more info.
			flag_share_mode = 0x008,

			// determines if the IP filter should apply to this torrent or not. By
			// default all torrents are subject to filtering by the IP filter
			// (i.e. this flag is set by default). This is useful if certain
			// torrents needs to be exempt for some reason, being an auto-update
			// torrent for instance.
			flag_apply_ip_filter = 0x010,

			// specifies whether or not the torrent is to be started in a paused
			// state. I.e. it won't connect to the tracker or any of the peers
			// until it's resumed. This is typically a good way of avoiding race
			// conditions when setting configuration options on torrents before
			// starting them.
			flag_paused = 0x020,

			// If the torrent is auto-managed (``flag_auto_managed``), the torrent
			// may be resumed at any point, regardless of how it paused. If it's
			// important to manually control when the torrent is paused and
			// resumed, don't make it auto managed.
			// 
			// If ``flag_auto_managed`` is set, the torrent will be queued,
			// started and seeded automatically by libtorrent. When this is set,
			// the torrent should also be started as paused. The default queue
			// order is the order the torrents were added. They are all downloaded
			// in that order. For more details, see queuing_.
			// 
			// If you pass in resume data, the auto_managed state of the torrent
			// when the resume data was saved will override the auto_managed state
			// you pass in here. You can override this by setting
			// ``override_resume_data``.
			flag_auto_managed = 0x040,
			flag_duplicate_is_error = 0x080,

#ifndef TORRENT_NO_DEPRECATE
			// defaults to on and specifies whether tracker URLs loaded from
			// resume data should be added to the trackers in the torrent or
			// replace the trackers. When replacing trackers (i.e. this flag is not
			// set), any trackers passed in via add_torrent_params are also
			// replaced by any trackers in the resume data. The default behavior is
			// to have the resume data override the .torrent file _and_ the
			// trackers added in add_torrent_params.
			flag_merge_resume_trackers = 0x100,
#endif

			// on by default and means that this torrent will be part of state
			// updates when calling post_torrent_updates().
			flag_update_subscribe = 0x200,

			// sets the torrent into super seeding mode. If the torrent is not a
			// seed, this flag has no effect. It has the same effect as calling
			// ``torrent_handle::super_seeding(true)`` on the torrent handle
			// immediately after adding it.
			flag_super_seeding = 0x400,

			// sets the sequential download state for the torrent. It has the same
			// effect as calling ``torrent_handle::sequential_download(true)`` on
			// the torrent handle immediately after adding it.
			flag_sequential_download = 0x800,

#ifndef TORRENT_NO_DEPRECATE
			// if this flag is set, the save path from the resume data file, if
			// present, is honored. This defaults to not being set, in which
			// case the save_path specified in add_torrent_params is always used.
			flag_use_resume_save_path = 0x1000,
#endif

			// indicates that this torrent should never be unloaded from RAM, even
			// if unloading torrents are allowed in general. Setting this makes
			// the torrent exempt from loading/unloading management.
			flag_pinned = 0x2000,

#ifndef TORRENT_NO_DEPRECATE
			// defaults to on and specifies whether web seed URLs loaded from
			// resume data should be added to the ones in the torrent file or
			// replace them. No distinction is made between the two different kinds
			// of web seeds (`BEP 17`_ and `BEP 19`_). When replacing web seeds
			// (i.e. when this flag is not set), any web seeds passed in via
			// add_torrent_params are also replaced. The default behavior is to
			// have any web seeds in the resume data take precedence over whatever
			// is passed in here as well as the .torrent file.
			flag_merge_resume_http_seeds = 0x2000,
#endif

			// the stop when ready flag. Setting this flag is equivalent to calling
			// torrent_handle::stop_when_ready() immediately after the torrent is
			// added.
			flag_stop_when_ready = 0x4000,

			// when this flag is set, the tracker list in the add_torrent_params
			// object override any trackers from the torrent file. If the flag is
			// not set, the trackers from the add_torrent_params object will be
			// added to the list of trackers used by the torrent.
			flag_override_trackers = 0x8000,

			// If this flag is set, the web seeds from the add_torrent_params
			// object will override any web seeds in the torrent file. If it's not
			// set, web seeds in the add_torrent_params object will be added to the
			// list of web seeds used by the torrent.
			flag_override_web_seeds = 0x10000,

			// if this flag is set (which it is by default) the torrent will be
			// considered needing to save its resume data immediately as it's
			// added. New torrents that don't have any resume data should do that.
			// This flag is cleared by a successful call to read_resume_data()
			flag_need_save_resume = 0x20000,

			// internal
			default_flags = flag_pinned | flag_update_subscribe
			| flag_auto_managed | flag_paused | flag_apply_ip_filter
			| flag_need_save_resume
#ifndef TORRENT_NO_DEPRECATE
			| flag_merge_resume_http_seeds
			| flag_merge_resume_trackers
#endif
		};
	}
}