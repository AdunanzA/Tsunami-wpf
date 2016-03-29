#include "feed_settings.h"


#include "interop.h"
#include "add_torrent_params.h"

#define FS_PROP_IMPL(type, name) \
    type FeedSettings::name::get() \
    { \
        return feed_settings_->name; \
    } \
    void FeedSettings::name::set(type val) \
    { \
        feed_settings_->name = val; \
    }


using namespace Tsunami::Core;

FeedSettings::FeedSettings(libtorrent::feed_settings & settings)
{
	feed_settings_ = new libtorrent::feed_settings(settings);
}

FeedSettings::FeedSettings()
{
	feed_settings_ = new libtorrent::feed_settings();
}


FeedSettings::~FeedSettings()
{
}

libtorrent::feed_settings * FeedSettings::ptr()
{
	return feed_settings_;
}

AddTorrentParams ^ FeedSettings::add_args::get()
{
	return gcnew AddTorrentParams(feed_settings_->add_args);
}


void FeedSettings::url::set(System::String^ val)
{
	feed_settings_->url = interop::to_std_string(val);
}

System::String ^ FeedSettings::url::get()
{
	return interop::from_std_string(feed_settings_->url);
}

FS_PROP_IMPL(bool, auto_download);
FS_PROP_IMPL(bool, auto_map_handles);
FS_PROP_IMPL(int, default_ttl);




