#include "file_slice.h"

using namespace Tsunami::Core;

FileSlice::FileSlice()
{
	file_slice_ = new libtorrent::file_slice();
}


FileSlice::FileSlice(libtorrent::file_slice & slice)
{
	file_slice_ = new libtorrent::file_slice(slice);
}


FileSlice::~FileSlice()
{
	delete file_slice_;
}

libtorrent::file_slice * FileSlice::ptr()
{
	return file_slice_;
}

int FileSlice::file_index::get()
{
	return file_slice_->file_index;
}

void FileSlice::file_index::set(int value)
{
	file_slice_->file_index = value;
}

long long FileSlice::offset::get()
{
	return file_slice_->offset;
}

void FileSlice::offset::set(long long value)
{
	file_slice_->offset = value;
}

long long FileSlice::size::get()
{
	return file_slice_->size;
}

void FileSlice::size::set(long long value)
{
	file_slice_->size = value;
}
