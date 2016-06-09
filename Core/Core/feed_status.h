#pragma once

#include <libtorrent/rss.hpp>

namespace Tsunami
{
	namespace Core
	{
		ref class FeedItem;
		ref class ErrorCode;

		public ref class FeedStatus
		{
			internal:
				FeedStatus(libtorrent::feed_status & status);
			public:
				~FeedStatus();

				property System::String ^ url { System::String ^ get(); }
				property System::String ^ title { System::String ^ get(); }
				property System::String ^ description { System::String ^ get(); }
				property System::DateTime last_update { System::DateTime get(); } 
				property int next_update { int get(); }
				property bool updating { bool get(); }
				cli::array<FeedItem^>^ items();
				property ErrorCode ^ error { ErrorCode ^ get(); }
				property int ttl { int get(); }
				

			private:
				libtorrent::feed_status * feed_status_;
				libtorrent::feed_status * ptr();
		};
	}
}
