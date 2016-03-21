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
	}
}