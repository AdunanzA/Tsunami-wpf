#pragma once

#include <libtorrent/rss.hpp>

namespace Tsunami
{
	namespace Core
	{
		ref class FeedStatus;
		ref class FeedSettings;
		ref class Feed;

		public ref class FeedHandle
		{
		internal:
			FeedHandle(libtorrent::feed_handle & handle);
			libtorrent::feed_handle * ptr();
		public:
			FeedHandle();
			~FeedHandle();

			void update_feed();
			FeedStatus ^ get_feed_status();
		private:
			libtorrent::feed_handle * feed_handle_;
			
		};
	}
}

