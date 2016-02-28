#include "file_storage.h"



#include "file_entry.h"
#include "interop.h"

using namespace Tsunami::Core;

FileStorage::FileStorage(const libtorrent::file_storage& storage)
{
    storage_ = new libtorrent::file_storage(storage);
}

FileStorage::FileStorage()
{
    storage_ = new libtorrent::file_storage();
}

FileStorage::~FileStorage()
{
    delete storage_;
}

libtorrent::file_storage& FileStorage::ptr()
{
    return *storage_;
}

bool FileStorage::is_valid()
{
    return storage_->is_valid();
}

void FileStorage::reserve(int num_files)
{
    storage_->reserve(num_files);
}

void FileStorage::add_file(System::String^ p, long long size)
{
    storage_->add_file(interop::to_std_string(p), size);
}

void FileStorage::add_file(FileEntry^ entry)
{
    storage_->add_file(entry->ptr());
}

void FileStorage::rename_file(int index, System::String^ new_filename)
{
    storage_->rename_file(index, interop::to_std_string(new_filename));
}

int FileStorage::num_files()
{
    return storage_->num_files();
}

FileEntry^ FileStorage::at(int index)
{
    return gcnew FileEntry(storage_->at(index));
}

long long FileStorage::total_size()
{
    return storage_->total_size();
}

void FileStorage::set_num_pieces(int n)
{
    storage_->set_num_pieces(n);
}

int FileStorage::num_pieces()
{
    return storage_->num_pieces();
}

void FileStorage::set_piece_length(int l)
{
    storage_->set_piece_length(l);
}

int FileStorage::piece_length()
{
    return storage_->piece_length();
}

int FileStorage::piece_size(int index)
{
    return storage_->piece_size(index);
}

void FileStorage::set_name(System::String^ name)
{
    storage_->set_name(interop::to_std_string(name));
}

System::String^ FileStorage::name()
{
    return interop::from_std_string(storage_->name());
}

void FileStorage::optimize(int pad_file_limit, int alignment)
{
    storage_->optimize(pad_file_limit, alignment);
}

long long FileStorage::file_size(int index)
{
    return storage_->file_size(index);
}

System::String^ FileStorage::file_name(int index)
{
    return interop::from_std_string(storage_->file_name(index));
}

long long FileStorage::file_offset(int index)
{
    return storage_->file_offset(index);
}

bool FileStorage::pad_file_at(int index)
{
    return storage_->pad_file_at(index);
}

System::String^ FileStorage::symlink(int index)
{
    return interop::from_std_string(storage_->symlink(index));
}

System::String^ FileStorage::file_path(int index, System::String^ save_path)
{
    return interop::from_std_string(storage_->file_path(
        index,
        interop::to_std_string(save_path)));
}

int FileStorage::file_flags(int index)
{
    return storage_->file_flags(index);
}

void FileStorage::set_file_base(int index, long long offset)
{
    storage_->set_file_base(index, offset);
}

long long FileStorage::file_base(int index)
{
    return storage_->file_base(index);
}

int FileStorage::file_index_at_offset(long long offset)
{
    return storage_->file_index_at_offset(offset);
}
