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
	}
}