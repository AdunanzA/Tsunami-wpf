#pragma once

#include <libtorrent/bitfield.hpp>


namespace Tsunami
{
	namespace Core
	{

		public ref class BitField
		{
			public:
				BitField();
				BitField(int bits);
				BitField(int bits, bool val);
				~BitField();

				bool operator[](int index);
				bool get_bit(int index);
				void clear_bit(int index);
				void set_bit(int index);
				property bool all_set { bool get(); }
				property bool none_set { bool get(); }
				property int size { int get(); }
				property int num_words { int get(); }
				property bool empty { bool get(); }
			internal:
				BitField(libtorrent::bitfield & rhs);
				BitField(char * b, int bits);
				libtorrent::bitfield *bitfield_;
				libtorrent::bitfield *ptr();
		};
	}
}

