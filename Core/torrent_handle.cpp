#include "torrent_handle.h"



#include "announce_entry.h"
#include "interop.h"
#include "peer_info.h"
#include "sha1_hash.h"
#include "torrent_info.h"
#include "torrent_status.h"

using namespace Tsunami;

TorrentHandle::TorrentHandle(libtorrent::torrent_handle& handle)
{
    handle_ = new libtorrent::torrent_handle(handle);
}

TorrentHandle::~TorrentHandle()
{
    delete handle_;
}

libtorrent::torrent_handle* TorrentHandle::ptr()
{
    return handle_;
}

void TorrentHandle::add_piece(int piece, cli::array<System::Byte>^ data, int flags)
{
    pin_ptr<unsigned char> ptr = &data[0];
    const char *pbegin = (const char*)(const unsigned char*)ptr;
    handle_->add_piece(piece, pbegin, flags);
}

void TorrentHandle::read_piece(int index)
{
    handle_->read_piece(index);
}

bool TorrentHandle::have_piece(int index)
{
    return handle_->have_piece(index);
}

cli::array<PeerInfo^>^ TorrentHandle::get_peer_info()
{
    std::vector<libtorrent::peer_info> p;
    handle_->get_peer_info(p);

    cli::array<PeerInfo^>^ result = gcnew cli::array<PeerInfo^>(p.size());

    for (size_t i = 0; i < p.size(); i++)
    {
        result[i] = gcnew PeerInfo(p[i]);
    }

    return result;
}

TorrentStatus^ TorrentHandle::status()
{
    return gcnew TorrentStatus(handle_->status());
}

void TorrentHandle::reset_piece_deadline(int index)
{
    handle_->reset_piece_deadline(index);
}

void TorrentHandle::clear_piece_deadlines()
{
    handle_->clear_piece_deadlines();
}

void TorrentHandle::set_piece_deadline(int index, int deadline)
{
    handle_->set_piece_deadline(index, deadline);
}

void TorrentHandle::set_priority(int priority)
{
    handle_->set_priority(priority);
}

cli::array<long long>^ TorrentHandle::file_progress(int flags)
{
    std::vector<int64_t> progress;
    handle_->file_progress(progress, flags);

    cli::array<long long>^ result = gcnew cli::array<long long>(progress.size());
    for (size_t i = 0; i < progress.size(); i++)
    {
        result[i] = progress[i];
    }

    return result;
}

void TorrentHandle::clear_error()
{
    handle_->clear_error();
}

cli::array<AnnounceEntry^>^ TorrentHandle::trackers()
{
    std::vector<libtorrent::announce_entry> trackers = handle_->trackers();
    cli::array<AnnounceEntry^>^ result = gcnew cli::array<AnnounceEntry^>(trackers.size());

    for (size_t i = 0; i < trackers.size(); i++)
    {
        result[i] = gcnew AnnounceEntry(trackers[i]);
    }

    return result;
}

void TorrentHandle::add_tracker(AnnounceEntry^ entry)
{
    handle_->add_tracker(*entry->ptr());
}

void TorrentHandle::add_url_seed(System::String^ url)
{
    handle_->add_url_seed(interop::to_std_string(url));
}

void TorrentHandle::remove_url_seed(System::String^ url)
{
    handle_->remove_url_seed(interop::to_std_string(url));
}

cli::array<System::String^>^ TorrentHandle::url_seeds()
{
    std::set<std::string> url_seeds = handle_->url_seeds();
    cli::array<System::String^>^ result = gcnew cli::array<System::String^>(url_seeds.size());

    for (size_t i = 0; i < url_seeds.size(); i++)
    {
        result[i] = interop::from_std_string(
            *std::next(url_seeds.begin(), i));
    }

    return result;
}

void TorrentHandle::add_http_seed(System::String^ url)
{
    handle_->add_http_seed(interop::to_std_string(url));
}

void TorrentHandle::remove_http_seed(System::String^ url)
{
    handle_->remove_http_seed(interop::to_std_string(url));
}

cli::array<System::String^>^ TorrentHandle::http_seeds()
{
    std::set<std::string> http_seeds = handle_->http_seeds();
    cli::array<System::String^>^ result = gcnew cli::array<System::String^>(http_seeds.size());

    for (size_t i = 0; i < http_seeds.size(); i++)
    {
        result[i] = interop::from_std_string(
            *std::next(http_seeds.begin(), i));
    }

    return result;
}

void TorrentHandle::set_metadata(cli::array<System::Byte>^ metadata)
{
    pin_ptr<unsigned char> ptr = &metadata[0];
    const char *pbegin = (const char*)(const unsigned char*)ptr;
    handle_->set_metadata(pbegin, metadata->Length);
}

bool TorrentHandle::is_valid()
{
    return handle_->is_valid();
}

void TorrentHandle::pause()
{
    handle_->pause();
}

void TorrentHandle::resume()
{
    handle_->resume();
}

void TorrentHandle::set_upload_mode(bool m)
{
    handle_->set_upload_mode(m);
}

void TorrentHandle::set_share_mode(bool m)
{
    handle_->set_share_mode(m);
}

void TorrentHandle::flush_cache()
{
    handle_->flush_cache();
}

void TorrentHandle::apply_ip_filter(bool m)
{
    handle_->apply_ip_filter(m);
}

void TorrentHandle::force_recheck()
{
    handle_->force_recheck();
}

void TorrentHandle::save_resume_data(int flags)
{
    handle_->save_resume_data(flags);
}

bool TorrentHandle::need_save_resume_data()
{
    return handle_->need_save_resume_data();
}

void TorrentHandle::auto_managed(bool b)
{
    handle_->auto_managed(b);
}

void TorrentHandle::queue_position_down()
{
    handle_->queue_position_down();
}

void TorrentHandle::queue_position_top()
{
    handle_->queue_position_top();
}

int TorrentHandle::queue_position()
{
    return handle_->queue_position();
}

void TorrentHandle::queue_position_bottom()
{
    handle_->queue_position_bottom();
}

void TorrentHandle::queue_position_up()
{
    handle_->queue_position_up();
}

void TorrentHandle::resolve_countries(bool r)
{
    handle_->resolve_countries(r);
}

bool TorrentHandle::resolve_countries()
{
    return handle_->resolve_countries();
}

void TorrentHandle::set_ssl_certificate(System::String^ certificate, System::String^ private_key, System::String^ dh_params, System::String^ passphrase)
{
    handle_->set_ssl_certificate(
        interop::to_std_string(certificate),
        interop::to_std_string(private_key),
        interop::to_std_string(dh_params),
        interop::to_std_string(passphrase));
}

TorrentInfo^ TorrentHandle::torrent_file()
{
    if (!handle_->torrent_file())
    {
        return nullptr;
    }

    return gcnew TorrentInfo(*handle_->torrent_file().get());
}

cli::array<int>^ TorrentHandle::piece_availability()
{
    std::vector<int> avail;
    handle_->piece_availability(avail);

    cli::array<int>^ result = gcnew cli::array<int>(avail.size());
    for (size_t i = 0; i < avail.size(); i++)
    {
        result[i] = avail[i];
    }

    return result;
}

int TorrentHandle::piece_priority(int index)
{
    return handle_->piece_priority(index);
}

void TorrentHandle::piece_priority(int index, int priority)
{
    handle_->piece_priority(index, priority);
}

cli::array<int>^ TorrentHandle::piece_priorities()
{
    std::vector<int> prio = handle_->piece_priorities();
    cli::array<int>^ result = gcnew cli::array<int>(prio.size());

    for (size_t i = 0; i < prio.size(); i++)
    {
        result[i] = prio[i];
    }

    return result;
}

int TorrentHandle::file_priority(int index)
{
    return handle_->file_priority(index);
}

void TorrentHandle::prioritize_files(cli::array<int>^ files)
{
    std::vector<int> f;
    f.reserve(files->Length);

    for (int i = 0; i < files->Length; i++)
    {
        f[i] = files[i];
    }

    handle_->prioritize_files(f);
}

void TorrentHandle::file_priority(int index, int priority)
{
    handle_->file_priority(index, priority);
}

cli::array<int>^ TorrentHandle::file_priorities()
{
    std::vector<int> prios = handle_->file_priorities();
    cli::array<int>^ result = gcnew cli::array<int>(prios.size());

    for (size_t i = 0; i < prios.size(); i++)
    {
        result[i] = prios[i];
    }

    return result;
}

void TorrentHandle::force_reannounce(int seconds, int tracker_index)
{
    handle_->force_reannounce(seconds, tracker_index);
}

void TorrentHandle::force_dht_announce()
{
    handle_->force_dht_announce();
}

void TorrentHandle::scrape_tracker()
{
    handle_->scrape_tracker();
}

int TorrentHandle::upload_limit()
{
    return handle_->upload_limit();
}

int TorrentHandle::download_limit()
{
    return handle_->download_limit();
}

void TorrentHandle::set_upload_limit(int limit)
{
    handle_->set_upload_limit(limit);
}

void TorrentHandle::set_download_limit(int limit)
{
    handle_->set_download_limit(limit);
}

void TorrentHandle::set_sequential_download(bool m)
{
    handle_->set_sequential_download(m);
}

int TorrentHandle::max_uploads()
{
    return handle_->max_uploads();
}

void TorrentHandle::set_max_uploads(int max_uploads)
{
    handle_->set_max_uploads(max_uploads);
}

int TorrentHandle::max_connections()
{
    return handle_->max_connections();
}

void TorrentHandle::set_max_connections(int max_connections)
{
    handle_->set_max_connections(max_connections);
}

void TorrentHandle::set_tracker_login(System::String^ name, System::String^ password)
{
    handle_->set_tracker_login(
        interop::to_std_string(name),
        interop::to_std_string(password));
}

void TorrentHandle::move_storage(System::String^ save_path, int flags)
{
    handle_->move_storage(
        interop::to_std_string(save_path),
        flags);
}

void TorrentHandle::rename_file(int index, System::String^ name)
{
    handle_->rename_file(
        index,
        interop::to_std_string(name));
}

void TorrentHandle::super_seeding(bool on)
{
    handle_->super_seeding(on);
}

Sha1Hash^ TorrentHandle::info_hash()
{
    return gcnew Sha1Hash(handle_->info_hash());
}
