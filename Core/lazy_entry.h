#pragma once

#include <libtorrent/lazy_entry.hpp>


namespace Tsunami
{
	namespace Core
	{
		public ref class LazyEntry
		{
		internal:
			LazyEntry(libtorrent::lazy_entry& le);
			libtorrent::lazy_entry* ptr();

		public:
			~LazyEntry();

		private:
			libtorrent::lazy_entry* entry_;
		};
	}
}
