#pragma once

#include <libtorrent/entry.hpp>

using namespace System::Collections;

namespace Tsunami
{
	namespace Core
	{
		public ref class Entry
		{
		internal:
			Entry(const libtorrent::entry& e);
			libtorrent::entry* ptr();

		public:
			~Entry();
			Entry(System::Collections::Generic::Dictionary<System::String ^, Entry ^> ^);
			Entry(System::String ^);
			Entry(System::Collections::Generic::List<Entry ^> ^);
			Entry(int);

		enum class data_type
		{
			int_t,
			string_t,
			list_t,
			dictionary_t,
			undefined_t
		};

		data_type type()
		{
			return static_cast<data_type>(entry_->type());
		}

		private:
			libtorrent::entry* entry_;
		};
	}
}
