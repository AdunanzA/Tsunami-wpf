#pragma once

#include <libtorrent/alert_types.hpp>

namespace Tsunami
{
	namespace Core
	{
		/// <summary>
		/// holds statistics about a current dht_lookup operation.
		/// a DHT lookup is the traversal of nodes, looking up a
		/// set of target nodes in the DHT for retrieving and possibly
		/// storing information in the DHT
		/// </summary>
		public ref class DhtLookup
		{
			public:
				DhtLookup();
				DhtLookup(const libtorrent::dht_lookup & dht_lookup);
				~DhtLookup();

			/// <summary>
			/// string literal indicating which kind of lookup this is
			/// </summary>
			property System::String ^ type
			{
				System::String^ get();
			}

			/// <summary>
			/// the number of outstanding request to individual nodes
			/// this lookup has right now
			/// </summary>
			property int outstanding_requests
			{
				int get() { return dht_lookup_->outstanding_requests; };
			}

			/// <summary>
			/// the total number of requests that have timed out so far
			/// for this lookup
			/// </summary>
			property int timeouts
			{
				int get() { return dht_lookup_->timeouts; };
			}

			/// <summary>
			/// the total number of responses we have received for this
			/// lookup so far for this lookup
			/// </summary>
			property int responses
			{
				int get() { return dht_lookup_->responses; };
			}

			/// <summary>
			/// the branch factor for this lookup. This is the number of
			/// nodes we keep outstanding requests to in parallel by default.
			/// when nodes time out we may increase this.
			/// </summary>
			property int branch_factor
			{
				int get() { return dht_lookup_->branch_factor; };
			}

			/// <summary>
			/// the number of nodes left that could be queries for this
			/// lookup. Many of these are likely to be part of the trail
			/// while performing the lookup and would never end up actually
			/// being queried.
			/// </summary>
			property int nodes_left
			{
				int get() { return dht_lookup_->nodes_left; };
			}

			/// <summary>
			/// the number of seconds ago the
			/// last message was sent that's still
			/// outstanding
			/// </summary>
			property int last_sent
			{
				int get() { return dht_lookup_->last_sent; };
			}

			/// <summary>
			/// the number of outstanding requests
			/// that have exceeded the short timeout
			/// and are considered timed out in the
			/// sense that they increased the branch
			/// factor
			/// </summary>
			property int first_timeout
			{
				int get() { return dht_lookup_->first_timeout; };
			}

			private:
			libtorrent::dht_lookup * dht_lookup_;
		};
	}
}
