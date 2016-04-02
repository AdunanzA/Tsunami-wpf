#include "torrent_info.h"

#include <msclr/marshal_cppstd.h>

#include "announce_entry.h"
#include "interop.h"
#include "sha1_hash.h"
#include "file_storage.h"

using namespace Tsunami::Core;

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

FileStorage ^ TorrentInfo::files()
{
	return gcnew FileStorage(info_->files());
}

FileStorage ^ TorrentInfo::orig_files()
{
	return gcnew FileStorage(info_->orig_files());
}

System::String ^ TorrentInfo::file_path(int index)
{
	return interop::from_std_string(info_->files().file_path(index));
}

cli::array<System::String^>^ TorrentInfo::file_list()
{
	size_t files = info_->num_files();
	cli::array<System::String^>^ result = gcnew cli::array<System::String^>(files);
	
	for (size_t i = 0; i < files; i++)
	{
		result[i] = gcnew System::String(file_path(i));
	}
	return result;
}

int64_t TorrentInfo::file_size(int index)
{
	return info_->files().file_size(index);
}

void TorrentInfo::rename_file(int index, System::String^ new_filename)
{
    info_->rename_file(index, interop::to_std_string(new_filename));
}

void TorrentInfo::remap_files(FileStorage ^ f)
{
	return info_->remap_files(f->ptr());
}

cli::array<AnnounceEntry^>^ TorrentInfo::trackers()
{
    auto trackers = info_->trackers();
	size_t size = trackers.size();
    cli::array<AnnounceEntry^>^ result = gcnew cli::array<AnnounceEntry^>(size);

    for (size_t i = 0; i < size; i++)
    {
        result[i] = gcnew AnnounceEntry(trackers[i]);
    }

    return result;
}

void TorrentInfo::add_tracker(System::String^ url, int tier)
{
    info_->add_tracker(interop::to_std_string(url), tier);
}

cli::array<Sha1Hash^>^ TorrentInfo::similar_torrents()
{
	auto similar = info_->similar_torrents();
	size_t size = similar.size();
	cli::array<Sha1Hash^> ^ similarTorrents = gcnew cli::array<Sha1Hash^>(size);

	for (size_t i = 0; i < size; i++)
	{
		similarTorrents[i] = gcnew Sha1Hash(similar[i]);
	}
	return similarTorrents;
}

cli::array<System::String^>^ TorrentInfo::collections()
{
	auto collect = info_->collections();
	size_t size = collect.size();
	cli::array<System::String^>^ collections = gcnew cli::array<System::String^>(size);
	for (size_t i = 0; i < size; i++)
	{
		collections[i] = interop::from_std_string(collect[i]);
	}
	return collections;
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
	auto date = info_->creation_date();
	if (date)
	{
		return gcnew System::Nullable<System::DateTime>(System::DateTime(1970, 1, 1, 0, 0, 0, 0, System::DateTimeKind::Utc).AddSeconds((double)(*date)).ToLocalTime());
	}
	else return gcnew System::Nullable<System::DateTime>();
}

System::String^ TorrentInfo::name()
{
    return interop::from_std_string(info_->name());
}

System::String^ TorrentInfo::comment()
{
    return interop::from_std_string(info_->comment());
}

System::String^ TorrentInfo::creator()
{
    return interop::from_std_string(info_->creator());
}

int TorrentInfo::metadata_size()
{
    return info_->metadata_size();
}

cli::array<System::Byte>^ TorrentInfo::metadata()
{
	auto meta = info_->metadata();
	size_t size = info_->metadata_size();
	cli::array<System::Byte> ^ metadata_ = gcnew cli::array<System::Byte>(size);
	for (size_t i = 0; i < size; i++)
	{
		metadata_[i] = meta[i];
	}
	return metadata_;
}

bool TorrentInfo::is_merkle_torrent()
{
    return info_->is_merkle_torrent();
}

libtorrent::torrent_info* TorrentInfo::ptr()
{
    return info_;
}