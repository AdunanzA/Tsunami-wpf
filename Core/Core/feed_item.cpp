#include "feed_item.h"


#include "interop.h"
#include "torrent_handle.h"
#include "sha1_hash.h"

using namespace Tsunami::Core;


FeedItem::FeedItem(libtorrent::feed_item & feed_item)
{
	feed_item_ = new libtorrent::feed_item(feed_item);
}

FeedItem::FeedItem()
{
	feed_item_ = new libtorrent::feed_item();
}

FeedItem::~FeedItem()
{
	delete feed_item_;
}

libtorrent::feed_item * FeedItem::ptr()
{
	return feed_item_;
}

System::String ^ FeedItem::url::get()
{
	return interop::from_std_string(feed_item_->url);
}

System::String ^ FeedItem::uuid::get()
{
	return interop::from_std_string(feed_item_->uuid);
}

System::String ^ FeedItem::title::get()
{
	return interop::from_std_string(feed_item_->title);
}

System::String ^ FeedItem::description::get()
{
	return interop::from_std_string(feed_item_->description);
}

System::String ^ FeedItem::comment::get()
{
	return interop::from_std_string(feed_item_->comment);
}

System::String ^ FeedItem::category::get()
{
	return interop::from_std_string(feed_item_->category);
}

long long FeedItem::size::get()
{
	return feed_item_->size;
}

TorrentHandle^ FeedItem::handle::get()
{
	if(feed_item_->handle.is_valid())
		return gcnew TorrentHandle(feed_item_->handle);
	return nullptr;
}

Sha1Hash ^ FeedItem::info_hash::get()
{
	return gcnew Sha1Hash(feed_item_->info_hash);
}