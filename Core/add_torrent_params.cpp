#include "add_torrent_params.h"


#include <msclr/marshal_cppstd.h>

#include "interop.h"
#include "torrent_info.h"
#include "sha1_hash.h"

using namespace Tsunami::Core;

AddTorrentParams::AddTorrentParams()
{
    params_ = new libtorrent::add_torrent_params();
}

AddTorrentParams::AddTorrentParams(libtorrent::add_torrent_params & params)
{
	params_ = new libtorrent::add_torrent_params(params);
}

AddTorrentParams::~AddTorrentParams()
{
    delete params_;
}

System::String^ AddTorrentParams::name::get()
{
    return interop::from_std_string(params_->name);
}

void AddTorrentParams::name::set(System::String^ value)
{
    params_->name = interop::to_std_string(value);
}

System::String^ AddTorrentParams::url::get()
{
	return interop::from_std_string(params_->url);
}

void AddTorrentParams::url::set(System::String^ value)
{
	params_->url = interop::to_std_string(value);
}

System::String^ AddTorrentParams::save_path::get()
{
    return interop::from_std_string(params_->save_path);
}

void AddTorrentParams::save_path::set(System::String^ value)
{
    params_->save_path = interop::to_std_string(value);
}

TorrentInfo^ AddTorrentParams::ti::get()
{
    if(params_->ti)
    {
        return gcnew TorrentInfo(*params_->ti.get());
    }

    return nullptr;
}

void AddTorrentParams::ti::set(TorrentInfo^ value)
{
    params_->ti = boost::shared_ptr<libtorrent::torrent_info>(new libtorrent::torrent_info(*value->ptr()));
}

libtorrent::add_torrent_params* AddTorrentParams::ptr()
{
    return params_;
}

ATPFlags AddTorrentParams::flags::get()
{
	return static_cast<ATPFlags>(params_->flags);
}

void AddTorrentParams::flags::set(ATPFlags flag)
{
	params_->flags = static_cast<libtorrent::add_torrent_params::flags_t>(flag);
}

Sha1Hash ^ AddTorrentParams::info_hash::get()
{
	return gcnew Sha1Hash(params_->info_hash);
}

void AddTorrentParams::info_hash::set(Sha1Hash ^ value)
{
	params_->info_hash = value->ptr();
}

cli::array<System::String ^> ^ AddTorrentParams::trackers::get()
{
	size_t size = params_->trackers.size();
	cli::array<System::String ^> ^ trk = gcnew cli::array<System::String ^>(size);
	for (size_t i = 0; i < size; i++)
	{
		trk[i] = interop::from_std_string(params_->trackers[i]);
	}
	return trk;
}

void AddTorrentParams::trackers::set(cli::array<System::String ^> ^ value)
{
	params_->trackers.clear();
	params_->trackers.reserve(value->Length);
	for each (System::String ^ tr in value)
	{
		params_->trackers.push_back(interop::to_std_string(tr));
	}
}


cli::array<int> ^ AddTorrentParams::tracker_tiers::get()
{
	size_t size = params_->tracker_tiers.size();
	cli::array<int> ^ tiers = gcnew cli::array<int>(size);
	for (size_t i = 0; i < size; i++)
	{
		tiers[i] = params_->tracker_tiers[i];
	}
	return tiers;
}

void AddTorrentParams::tracker_tiers::set(cli::array<int> ^ value)
{
	params_->tracker_tiers.clear();
	params_->tracker_tiers.reserve(value->Length);
	for each (int tier in value)
	{
		params_->tracker_tiers.push_back(tier);
	}
}

storage_mode_t AddTorrentParams::storage_mode::get()
{
	return static_cast<storage_mode_t>(params_->storage_mode);
}


void AddTorrentParams::storage_mode::set(storage_mode_t value)
{
	params_->storage_mode = static_cast<libtorrent::storage_mode_t>(value);
}

cli::array<unsigned> ^ AddTorrentParams::file_priorities::get()
{
	size_t size = params_->file_priorities.size();
	cli::array<unsigned> ^ priorities = gcnew cli::array<unsigned>(size);
	for (size_t i = 0; i < size; i++)
	{
		priorities[i] = params_->file_priorities[i];
	}
	return priorities;
}

void AddTorrentParams::file_priorities::set(cli::array<unsigned> ^ value)
{
	params_->file_priorities.clear();
	params_->file_priorities.reserve(value->Length);
	for each (unsigned i in value)
	{
		params_->file_priorities.push_back(i);
	}
}

System::DateTime AddTorrentParams::added_time::get()
{
	return System::DateTime(1970, 1, 1, 0, 0, 0, 0, System::DateTimeKind::Utc).AddSeconds((double)params_->added_time).ToLocalTime();
}

System::DateTime AddTorrentParams::completed_time::get()
{
	return System::DateTime(1970, 1, 1, 0, 0, 0, 0, System::DateTimeKind::Utc).AddSeconds((double)params_->completed_time).ToLocalTime();
}

System::DateTime AddTorrentParams::last_seen_complete::get()
{
	return System::DateTime(1970, 1, 1, 0, 0, 0, 0, System::DateTimeKind::Utc).AddSeconds((double)params_->last_seen_complete).ToLocalTime();
}


cli::array<System::String^> ^ AddTorrentParams::http_seed::get()
{
	size_t size = params_->http_seeds.size();
	cli::array<System::String^> ^ seed = gcnew cli::array<System::String ^>(size);
	for (size_t i = 0; i < size; i++)
	{
		seed[i] = interop::from_std_string(params_->http_seeds[i]);
	}
	return seed;
}

void AddTorrentParams::http_seed::set(cli::array<System::String ^> ^ value)
{
	params_->http_seeds.clear();
	params_->http_seeds.reserve(value->Length);
	for each (System::String ^ s  in value)
	{
		params_->http_seeds.push_back(interop::to_std_string(s));
	}
}

cli::array<System::String^> ^ AddTorrentParams::url_seeds::get()
{
	size_t size = params_->url_seeds.size();
	cli::array<System::String^> ^ seed = gcnew cli::array<System::String ^>(size);
	for (size_t i = 0; i < size; i++)
	{
		seed[i] = interop::from_std_string(params_->url_seeds[i]);
	}
	return seed;
}

void AddTorrentParams::url_seeds::set(cli::array<System::String ^> ^ value)
{
	params_->url_seeds.clear();
	params_->url_seeds.reserve(value->Length);
	for each (System::String ^ s  in value)
	{
		params_->url_seeds.push_back(interop::to_std_string(s));
	}
}

