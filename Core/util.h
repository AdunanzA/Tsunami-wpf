#pragma once

#include <libtorrent/bencode.hpp>
#include <libtorrent/lazy_entry.hpp>

namespace Tsunami
{
	namespace Core
	{
		ref class Entry;
		ref class LazyEntry;

		public ref class Util
		{
		public:
			static cli::array<System::Byte>^ bencode(Entry^ e);

			static LazyEntry^ lazy_bdecode(cli::array<System::Byte>^ buffer);
		};
	}
}
