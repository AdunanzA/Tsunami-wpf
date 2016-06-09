#pragma once

#include <libtorrent/file_storage.hpp>

namespace Tsunami
{
	namespace Core
	{
		public ref class FileSlice
		{
		internal:
			FileSlice(libtorrent::file_slice & slice);
		public:
			FileSlice();
			~FileSlice();
			// the index of the file
			property int file_index {int get(); void set(int value); }

			// the offset from the start of the file, in bytes
			property long long offset { long long get(); void set(long long value); }

			// the size of the window, in bytes
			property long long size { long long get(); void set(long long value); }			
			
		private:
			libtorrent::file_slice * file_slice_;
			libtorrent::file_slice * ptr();
		};
	}
}

