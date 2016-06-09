#pragma once

#include <libtorrent/rss.hpp>

namespace Tsunami
{
	namespace Core
	{
		ref class TorrentHandle;
		ref class Sha1Hash;

		public ref class FeedItem
		{
		internal:
			
			FeedItem(libtorrent::feed_item & feed_item);
		public:
			FeedItem();
			~FeedItem();

			property System::String ^ url { System::String ^ get(); }
			property System::String ^ uuid { System::String ^ get(); }
			property System::String ^ title { System::String ^ get(); }
			property System::String ^ description { System::String ^ get(); }
			property System::String ^ comment { System::String ^ get(); }
			property System::String ^ category { System::String ^ get(); }

			property long long size {long long get(); }
			property TorrentHandle^ handle {TorrentHandle ^ get(); }
			property Sha1Hash^ info_hash {Sha1Hash ^ get(); }
		internal:
			libtorrent::feed_item *ptr();
		private:
			libtorrent::feed_item *feed_item_;
		};
	}
}

