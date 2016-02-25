#pragma once

#include <libtorrent/file_storage.hpp>

#define FE_PROP(type, name) \
    property type name { type get(); void set(type val); }

namespace Tsunami
{
    public ref class FileEntry
    {
    internal:
        FileEntry(const libtorrent::file_entry& entry);
        libtorrent::file_entry& ptr();

    public:
        ~FileEntry();

        FE_PROP(System::String^, path);
        FE_PROP(System::String^, symlink_path);
        FE_PROP(long long, offset);
        FE_PROP(long long, size);
        FE_PROP(long long, file_base);
        // TODO mtime
        // TODO filehash
        FE_PROP(bool, pad_file);
        FE_PROP(bool, hidden_attribute);
        FE_PROP(bool, executable_attribute);
        FE_PROP(bool, symlink_attribute);

    private:
        libtorrent::file_entry* entry_;
    };
}
