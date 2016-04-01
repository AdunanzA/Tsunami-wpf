#include "feed_status.h"

#include "feed_item.h"
#include "interop.h"
#include "error_code.h"


using namespace Tsunami::Core;

FeedStatus::FeedStatus(libtorrent::feed_status & status)
{
	feed_status_ = new libtorrent::feed_status(status);
}


FeedStatus::~FeedStatus()
{
	delete feed_status_;
}

cli::array<FeedItem ^> ^ FeedStatus::items()
{
	size_t size = feed_status_->items.size();
	cli::array<FeedItem ^> ^ feed_items = gcnew cli::array<FeedItem ^ >(size);
	for (size_t i = 0; i < size; i++)
	{
		feed_items[i] = gcnew FeedItem(feed_status_->items[i]);
	}
	return feed_items;
}

System::String ^ FeedStatus::url::get()
{
	return interop::from_std_string(feed_status_->url);
}

System::String ^ FeedStatus::title::get()
{
	return interop::from_std_string(feed_status_->title);
}

System::String ^ FeedStatus::description::get()
{
	return interop::from_std_string(feed_status_->description);
}

System::DateTime FeedStatus::last_update::get()
{
	double msec = static_cast<double>(feed_status_->last_update);
	return System::DateTime(1970, 1, 1, 0, 0, 0, System::DateTimeKind::Utc).AddMilliseconds(msec);
}

int FeedStatus::next_update::get()
{
	return feed_status_->next_update;
}

bool FeedStatus::updating::get()
{
	return feed_status_->updating;
}

ErrorCode ^ FeedStatus::error::get()
{
	return gcnew ErrorCode(feed_status_->error);
}

int FeedStatus::ttl::get()
{
	return feed_status_->ttl;
}

libtorrent::feed_status * FeedStatus::ptr()
{
	return feed_status_;
}
