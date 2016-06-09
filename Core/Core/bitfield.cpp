#include "bitfield.h"

using namespace Tsunami::Core;

BitField::BitField()
{
	bitfield_ = new libtorrent::bitfield();
}

BitField::BitField(int bits)
{
	bitfield_ = new libtorrent::bitfield(bits);
}

BitField::BitField(int bits, bool val)
{
	bitfield_ = new libtorrent::bitfield(bits, val);
}


BitField::~BitField()
{
	delete bitfield_;
}

bool BitField::operator[](int index)
{
	return (*bitfield_)[index];
}

bool BitField::get_bit(int index)
{
	return bitfield_->get_bit(index);
}

void BitField::clear_bit(int index)
{
	bitfield_->clear_bit(index);
}

void BitField::set_bit(int index)
{
	bitfield_->set_bit(index);
}

bool BitField::all_set::get()
{
	return bitfield_->all_set();
}

bool BitField::none_set::get()
{
	return bitfield_->none_set();
}

int BitField::size::get()
{
	return bitfield_->size();
}

int BitField::num_words::get()
{
	return bitfield_->num_words();
}

bool  BitField::empty::get()
{
	return bitfield_->empty();
}

BitField::BitField(libtorrent::bitfield & rhs)
{
	bitfield_ = new libtorrent::bitfield(rhs);
}

BitField::BitField(char * b, int bits)
{
	bitfield_ = new libtorrent::bitfield(b, bits);
}

libtorrent::bitfield * BitField::ptr()
{
	return bitfield_;
}
