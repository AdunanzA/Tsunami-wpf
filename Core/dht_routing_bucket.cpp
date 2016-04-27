#include "dht_routing_bucket.h"

using namespace Tsunami::Core;


DhtRoutingBucket::DhtRoutingBucket()
{
	dht_routing_bucket_ = new libtorrent::dht_routing_bucket();
}

DhtRoutingBucket::DhtRoutingBucket(const libtorrent::dht_routing_bucket & dht_routing_bucket)
{
	dht_routing_bucket_ = new libtorrent::dht_routing_bucket(dht_routing_bucket);
}

DhtRoutingBucket::~DhtRoutingBucket()
{
	delete dht_routing_bucket_;
}
