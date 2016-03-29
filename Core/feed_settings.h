#pragma once

#include <libtorrent/rss.hpp>

#define FS_PROP(type, name) \
    property type name { type get(); void set(type val); }

namespace Tsunami
{
	namespace Core
	{
		ref class AddTorrentParams;

		public ref class FeedSettings
		{
			internal:
				FeedSettings(libtorrent::feed_settings & settings);
			public:
				FeedSettings();
				~FeedSettings();

				FS_PROP(System::String^, url);
				FS_PROP(bool, auto_download);
				FS_PROP(bool, auto_map_handles);
				FS_PROP(int, default_ttl);

				property AddTorrentParams^ add_args { AddTorrentParams ^ get(); }

				libtorrent::feed_settings *ptr();

			private:
				libtorrent::feed_settings *feed_settings_;
				
				
		};
	}
}

