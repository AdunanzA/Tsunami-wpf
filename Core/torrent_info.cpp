#include "torrent_info.h"

#include <msclr/marshal_cppstd.h>

#include "announce_entry.h"
#include "interop.h"
#include "sha1_hash.h"

using namespace Tsunami;

TorrentInfo::TorrentInfo(cli::array<System::Byte>^ buffer)
{
    pin_ptr<unsigned char> ptr = &buffer[0];
    const char *pbegin = (const char*)(const unsigned char*)ptr;
    info_ = new libtorrent::torrent_info(pbegin, buffer->Length);
}

TorrentInfo::TorrentInfo(System::String^ fileName)
{
    std::string file = interop::to_std_string(fileName);
    info_ = new libtorrent::torrent_info(file);
}

TorrentInfo::TorrentInfo(const libtorrent::torrent_info& info)
{
    info_ = new libtorrent::torrent_info(info);
}

TorrentInfo::~TorrentInfo()
{
    delete info_;
}

void TorrentInfo::rename_file(int index, System::String^ new_filename)
{
    info_->rename_file(index, interop::to_std_string(new_filename));
}

cli::array<AnnounceEntry^>^ TorrentInfo::trackers()
{
    std::vector<libtorrent::announce_entry> trackers = info_->trackers();
    cli::array<AnnounceEntry^>^ result = gcnew cli::array<AnnounceEntry^>(trackers.size());

    for (size_t i = 0; i < trackers.size(); i++)
    {
        result[i] = gcnew AnnounceEntry(trackers[i]);
    }

    return result;
}

void TorrentInfo::add_tracker(System::String^ url, int tier)
{
    info_->add_tracker(interop::to_std_string(url), tier);
}

int TorrentInfo::num_pieces()
{
    return info_->num_pieces();
}

long long TorrentInfo::total_size()
{
    return info_->total_size();
}

int TorrentInfo::piece_length()
{
    return info_->piece_length();
}

Sha1Hash^ TorrentInfo::info_hash()
{
    return gcnew Sha1Hash(info_->info_hash());
}

int TorrentInfo::num_files()
{
    return info_->num_files();
}

System::String^ TorrentInfo::ssl_cert()
{
    return interop::from_std_string(info_->ssl_cert());
}

bool TorrentInfo::is_valid()
{
    return info_->is_valid();
}

bool TorrentInfo::priv()
{
    return info_->priv();
}

bool TorrentInfo::is_i2p()
{
    return info_->is_i2p();
}

int TorrentInfo::piece_size(int index)
{
    return info_->piece_size(index);
}

System::Nullable<System::DateTime>^ TorrentInfo::creation_date()
{
    // TODO
    throw gcnew System::NotImplementedException();
}

System::String^ TorrentInfo::name()
{
    return Tsunami::interop::from_std_string(info_->name());
}

System::String^ TorrentInfo::comment()
{
    return Tsunami::interop::from_std_string(info_->comment());
}

System::String^ TorrentInfo::creator()
{
    return Tsunami::interop::from_std_string(info_->creator());
}

int TorrentInfo::metadata_size()
{
    return info_->metadata_size();
}

cli::array<System::Byte>^ TorrentInfo::metadata()
{
    // TODO
    throw gcnew System::NotImplementedException();
}

bool TorrentInfo::is_merkle_torrent()
{
    return info_->is_merkle_torrent();
}

libtorrent::torrent_info* TorrentInfo::ptr()
{
    return info_;
}