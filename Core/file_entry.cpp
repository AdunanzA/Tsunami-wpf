#include "file_entry.h"



#include "interop.h"

#define FE_PROP_IMPL(type, name) \
    type FileEntry::name::get() \
    { \
        return entry_->name; \
    } \
    void FileEntry::name::set(type val) \
    { \
        entry_->name = val; \
    }

using namespace Tsunami::Core;

FileEntry::FileEntry(const libtorrent::file_entry& entry)
{
    entry_ = new libtorrent::file_entry(entry);
}

FileEntry::~FileEntry()
{
    delete entry_;
}

libtorrent::file_entry& FileEntry::ptr()
{
    return *entry_;
}

System::String^ FileEntry::path::get()
{
    return interop::from_std_string(entry_->path);
}

void FileEntry::path::set(System::String^ val)
{
    entry_->path = interop::to_std_string(val);
}

System::String^ FileEntry::symlink_path::get()
{
    return interop::from_std_string(entry_->symlink_path);
}

void FileEntry::symlink_path::set(System::String^ val)
{
    entry_->symlink_path = interop::to_std_string(val);
}

FE_PROP_IMPL(long long, offset);
FE_PROP_IMPL(long long, size);
FE_PROP_IMPL(long long, file_base);
// TODO mtime
// TODO filehash
FE_PROP_IMPL(bool, pad_file);
FE_PROP_IMPL(bool, hidden_attribute);
FE_PROP_IMPL(bool, executable_attribute);
FE_PROP_IMPL(bool, symlink_attribute);
