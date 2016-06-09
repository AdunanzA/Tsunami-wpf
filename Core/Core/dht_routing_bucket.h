#pragma once

#include <libtorrent/alert_types.hpp>

namespace Tsunami
{
	namespace Core
	{
		/// <summary>
		/// struct to hold information about a single DHT routing table bucket
		/// </summary>
		public ref class DhtRoutingBucket
		{
		public:
			DhtRoutingBucket();
			DhtRoutingBucket(const libtorrent::dht_routing_bucket & dht_routing_bucket);
			~DhtRoutingBucket();
		
			/// <summary>
			/// the total number of nodes and replacement nodes
			/// in the routing table
			/// </summary>
			property int num_nodes
			{
				int get() { return dht_routing_bucket_->num_nodes; };
			}

			property int num_replacements
			{
				int get() { return dht_routing_bucket_->num_replacements; };
			}

			/// <summary>
			/// number of seconds since last activity
			/// </summary>
			property int last_active
			{
				int get() { return dht_routing_bucket_->last_active; };
			}

		private:
			libtorrent::dht_routing_bucket * dht_routing_bucket_;
		};
	}
}
