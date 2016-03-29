#pragma once

#include <libtorrent/add_torrent_params.hpp>
#include <libtorrent/torrent_info.hpp>

namespace Tsunami
{
	namespace Core
	{
		ref class TorrentInfo;

		public ref class AddTorrentParams
		{
		public:
			AddTorrentParams();
			~AddTorrentParams();


			property System::String^ name
			{
				System::String^ get();
				void set(System::String^ value);
			}

			property System::String^ url
			{
				System::String^ get();
				void set(System::String^ value);
			}

			property System::String^ save_path
			{
				System::String^ get();
				void set(System::String^ value);
			}

			property TorrentInfo^ ti
			{
				TorrentInfo^ get();
				void set(TorrentInfo^ value);
			}

		internal:
			libtorrent::add_torrent_params* ptr();
			AddTorrentParams(libtorrent::add_torrent_params & params);
		private:
			libtorrent::add_torrent_params* params_;
		};
	}
}
