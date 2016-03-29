#include "feed_handle.h"

#include "feed_status.h"
#include "feed_settings.h"

using namespace Tsunami::Core;


FeedHandle::FeedHandle(libtorrent::feed_handle & handle)
{
	feed_handle_ = new libtorrent::feed_handle(handle);
}

FeedHandle::FeedHandle()
{
	feed_handle_ = new libtorrent::feed_handle();
}

FeedHandle::~FeedHandle()
{
	delete feed_handle_;
}

void FeedHandle::update_feed()
{
	feed_handle_->update_feed();
}

FeedStatus ^ FeedHandle::get_feed_status()
{
	return gcnew FeedStatus(feed_handle_->get_feed_status());
}

libtorrent::feed_handle * FeedHandle::ptr()
{
	return feed_handle_;
}