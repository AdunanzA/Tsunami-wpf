#pragma once

#include <libtorrent/torrent_info.hpp>
#include <libtorrent/announce_entry.hpp>

namespace Tsunami
{
	namespace Core
	{
		public ref class AnnounceEntry
		{
		internal:
			AnnounceEntry(libtorrent::announce_entry& entry);
			libtorrent::announce_entry* ptr();

		public:
			AnnounceEntry(System::String^ url);
			~AnnounceEntry();

			int next_announce_in();
			int min_announce_in();
			void reset();
			// TODO failed
			bool can_announce(System::DateTime now, bool is_seed);
			bool is_working();
			void trim();

			property System::String^ url { System::String^ get(); }
			property System::String^ trackerid { System::String^ get(); }
			property System::String^ message { System::String^ get(); }
			// TODO last error
			// TODO property System::DateTime next_announce { System::DateTime get(); }
			// TODO property System::DateTime min_announce { System::DateTime get(); }
			property int scrape_incomplete { int get(); }
			property int scrape_complete { int get(); }
			property int scrape_downloaded { int get(); }
			property int tier { int get(); }
			property int fail_limit { int get(); }
			property int fails { int get(); }
			property bool updating { bool get(); }
			property int source { int get(); }
			property bool verified { bool get(); }
			property bool start_sent { bool get(); }
			property bool complete_sent { bool get(); }
			property bool send_stats { bool get(); }

		private:
			libtorrent::announce_entry* entry_;
		};
	}
}
