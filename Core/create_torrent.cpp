#include "create_torrent.h"



#include "entry.h"
#include "file_storage.h"
#include "interop.h"
#include "sha1_hash.h"

using namespace Tsunami::Core;

CreateTorrent::CreateTorrent(FileStorage^ fs)
    : CreateTorrent(fs, 0, -1, libtorrent::create_torrent::optimize, -1)
{
}

CreateTorrent::CreateTorrent(FileStorage^ fs, int piece_size)
    : CreateTorrent(fs, piece_size, -1, libtorrent::create_torrent::optimize, -1)
{
}

CreateTorrent::CreateTorrent(FileStorage^ fs, int piece_size, int pad_file_limit)
    : CreateTorrent(fs, piece_size, pad_file_limit, libtorrent::create_torrent::optimize, -1)
{
}

CreateTorrent::CreateTorrent(FileStorage^ fs, int piece_size, int pad_file_limit, int flags)
    : CreateTorrent(fs, piece_size, pad_file_limit, flags, -1)
{
}

CreateTorrent::CreateTorrent(FileStorage^ fs, int piece_size, int pad_file_limit, int flags, int alignment)
{
    create_ = new libtorrent::create_torrent(fs->ptr(), piece_size, pad_file_limit, flags, alignment);
}

CreateTorrent::~CreateTorrent()
{
    delete create_;
}

libtorrent::create_torrent& CreateTorrent::ptr()
{
    return *create_;
}

void CreateTorrent::add_files(FileStorage^ fs, System::String^ path)
{
    add_files(fs, path, 0U);
}

void CreateTorrent::add_files(FileStorage^ fs, System::String^ path, unsigned int flags)
{
    libtorrent::add_files(fs->ptr(), interop::to_std_string(path), flags);
}

void CreateTorrent::set_piece_hashes(CreateTorrent^ ct, System::String^ path)
{
    libtorrent::set_piece_hashes(ct->ptr(), interop::to_std_string(path));
}

Entry^ CreateTorrent::generate()
{
    return gcnew Entry(create_->generate());
}

FileStorage^ CreateTorrent::files()
{
    return gcnew FileStorage(create_->files());
}

void CreateTorrent::set_comment(System::String^ str)
{
    create_->set_comment(interop::to_std_string(str).c_str());
}

void CreateTorrent::set_creator(System::String^ str)
{
    create_->set_creator(interop::to_std_string(str).c_str());
}

void CreateTorrent::set_hash(int index, Sha1Hash^ hash)
{
    create_->set_hash(index, hash->ptr());
}

void CreateTorrent::set_file_hash(int index, Sha1Hash^ hash)
{
    create_->set_file_hash(index, hash->ptr());
}

void CreateTorrent::add_url_seed(System::String^ url)
{
    create_->add_url_seed(interop::to_std_string(url));
}

void CreateTorrent::add_http_seed(System::String^ url)
{
    create_->add_http_seed(interop::to_std_string(url));
}

void CreateTorrent::add_node(System::String^ host, int port)
{
    create_->add_node(std::make_pair(interop::to_std_string(host), port));
}

void CreateTorrent::add_tracker(System::String^ url)
{
    add_tracker(url, 0);
}

void CreateTorrent::add_tracker(System::String^ url, int tier)
{
    create_->add_tracker(interop::to_std_string(url), tier);
}

void CreateTorrent::set_root_cert(System::String^ pem)
{
    create_->set_root_cert(interop::to_std_string(pem));
}

bool CreateTorrent::priv()
{
    return create_->priv();
}

void CreateTorrent::set_priv(bool p)
{
    create_->set_priv(p);
}

int CreateTorrent::num_pieces()
{
    return create_->num_pieces();
}

int CreateTorrent::piece_length()
{
    return create_->piece_length();
}

int CreateTorrent::piece_size(int n)
{
    return create_->piece_size(n);
}
