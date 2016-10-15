#pragma once

#include <libtorrent/session.hpp>
#include <libtorrent/aux_/session_impl.hpp>
#include <libtorrent/session_status.hpp>
#include <libtorrent/session_stats.hpp>
#include "enums.h"


namespace Tsunami
{
	namespace Core
	{

		ref class Alert;
		ref class AddTorrentParams;
		ref class DhtSettings;
		ref class Entry;
		ref class LazyEntry;
		ref class SessionSettings;
		ref class SessionStatus;
		ref class Sha1Hash;
		ref class TorrentHandle;
		ref class FeedSettings;
		ref class FeedHandle;
		ref class StatsMetrics;

		class SessionAlertDispatcher;
		class SessionDispatcher;

		public ref class Session
		{
		public:

			Session();
			~Session();

			void load_state(LazyEntry^ e);
			Entry^ save_state(unsigned int flags);
			// TODO refresh_torrent_status
			// TODO get torrent status
			void post_torrent_updates();
			TorrentHandle^ find_torrent(Sha1Hash^ hash);
			cli::array<TorrentHandle^>^ get_torrents();
			void async_add_torrent(AddTorrentParams^ params);
			TorrentHandle^ add_torrent(AddTorrentParams^ params);
			// TODO abort
			void pause();
			void resume();
			bool is_paused();
			SessionStatus^ status();
			// TODO get cache status
			// TODO get cache info
			

			FeedHandle ^ add_feed(FeedSettings ^ feed);
			void remove_feed(FeedHandle ^ handle);
			cli::array<FeedHandle ^> ^ get_feeds();
			
			bool is_dht_running();
			DhtSettings^ get_dht_settings();
			void start_dht();
			void stop_dht();
			void set_dht_settings(DhtSettings^ settings);

			/// <summary>
			/// Takes a host name and port pair. That endpoint will be pinged, and
			/// if a valid DHT reply is received, the node will be added to the
			/// routing table.
			/// </summary>
			void add_dht_router(System::String^ host, int port);

			/// <summary>
			/// adds the given endpoint to a list of DHT router nodes. If a search
			/// is ever made while the routing table is empty, those nodes will be
			/// used as backups. Nodes in the router node list will also never be
			/// added to the regular routing table, which effectively means they
			/// are only used for bootstrapping, to keep the load off them.
			/// </summary>
			void add_dht_node(System::String^ host, int port);
			/// <summary>
			/// store the given bencoded data as an immutable item in the DHT.
			/// the returned hash is the key that is to be used to look the item
			/// up agan. It's just the sha-1 hash of the bencoded form of the
			/// structure.
			/// </summary>
			Sha1Hash ^ dht_put_item(Entry ^ entry);
			/// <summary>
			/// query the DHT for an immutable item at the ``target`` hash.
			/// the result is posted as a dht_immutable_item_alert.
			/// </summary>
			void dht_get_item(Sha1Hash ^ target);
			/// <summary>
			/// query the DHT for a mutable item under the public key ``key``.
			/// this is an ed25519 key. ``salt`` is optional and may be left
			/// as an empty string if no salt is to be used.
			/// if the item is found in the DHT, a dht_mutable_item_alert is
			/// posted.
			/// </summary>
			void dht_get_item(cli::array<char> ^ public_key, System::String ^ salt);

			// TODO add extension
			void load_country_db(System::String^ file);
			void load_asnum_db(System::String^ file);
			// TODO int as_for_ip(System::Net::IP)
			// TODO get ip filter
			// TODO set ip filter
			// TODO set port filter
			// TODO id
			// TODO set peer id
			void set_key(int key);
			void listen_on(int minPort, int maxPort /* TODO: network interface */);
			bool is_listening();
			int listen_port();
			int ssl_listen_port();
			void remove_torrent(TorrentHandle^ handle, int options);
			SessionSettings^ settings();
			void set_settings(SessionSettings^ settings);
			// TODO get pe settings
			// TODO set pe settings
			// TODO set proxy
			// TODO proxy
			// TODO i2p proxy
			// TODO set i2p proxy
			// TODO pop alerts
			// TODO pop alert
			// TODO wait for alert

			cli::array<StatsMetrics ^> ^ session_stats_metrics();
			void set_alert_mask(AlertMask mask);
			void clear_alert_callback();
			void stop_lsd();
			void start_lsd();
			void stop_upnp();
			void start_upnp();
			void delete_port_mapping(int handle);
			// TODO add port mapping
			void stop_natpmp();
			void start_natpmp();
			void post_dht_stats();
			void post_session_stats();
			void set_alert_callback(System::Action<Alert^>^ dispatch);
			void set_session_callback(System::Action ^ dispatch);
			void get_pending_alerts();
			
		private:
			SessionDispatcher * sessionDispatcher_;
			libtorrent::session* session_;
			std::vector<libtorrent::alert *> *m_alerts;
			System::Action <Alert ^> ^ alertDispatcher;
		};
	}

	
}
