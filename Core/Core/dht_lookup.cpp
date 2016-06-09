#include "dht_lookup.h"
#include "interop.h"

using namespace Tsunami::Core;

DhtLookup::DhtLookup()
{
	dht_lookup_ = new libtorrent::dht_lookup();
}

DhtLookup::DhtLookup(const libtorrent::dht_lookup & dht_lookup)
{
	dht_lookup_ = new libtorrent::dht_lookup(dht_lookup);
}

DhtLookup::~DhtLookup()
{
	delete dht_lookup_;
}

System::String ^ DhtLookup::type::get()
{
	return interop::from_std_string(dht_lookup_->type);
}
