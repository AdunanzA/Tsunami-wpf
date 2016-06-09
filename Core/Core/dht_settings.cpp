#include "dht_settings.h"

using namespace Tsunami::Core;

#define DS_PROP_IMPL(type, name) \
    type DhtSettings::name::get() \
    { \
        return settings_->name; \
    } \
    void DhtSettings::name::set(type val) \
    { \
        settings_->name = val; \
    }



DhtSettings::DhtSettings(libtorrent::dht_settings& settings)
{
    settings_ = new libtorrent::dht_settings(settings);
}

DhtSettings::~DhtSettings()
{
    delete settings_;
}

libtorrent::dht_settings& DhtSettings::ptr()
{
    return *settings_;
}

DS_PROP_IMPL(int, max_peers_reply);
DS_PROP_IMPL(int, search_branching);
DS_PROP_IMPL(int, max_fail_count);
DS_PROP_IMPL(int, max_torrents);
DS_PROP_IMPL(int, max_dht_items);
DS_PROP_IMPL(int, max_torrent_search_reply);
DS_PROP_IMPL(bool, restrict_routing_ips);
DS_PROP_IMPL(bool, restrict_search_ips);
DS_PROP_IMPL(bool, extended_routing_table);
DS_PROP_IMPL(bool, aggressive_lookups);
DS_PROP_IMPL(bool, privacy_lookups);
DS_PROP_IMPL(bool, enforce_node_id);
DS_PROP_IMPL(bool, ignore_dark_internet);
