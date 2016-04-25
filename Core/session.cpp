#include "session.h"

#include <msclr/marshal_cppstd.h>

#include "alert.h"
#include "add_torrent_params.h"
#include "dht_settings.h"
#include "entry.h"
#include "interop.h"
#include "lazy_entry.h"
#include "session_alert_dispatcher.h"
#include "session_settings.h"
#include "session_status.h"
#include "sha1_hash.h"
#include "torrent_handle.h"
#include "feed_handle.h"
#include "feed_settings.h"

using namespace Tsunami::Core;

Session::Session()
{
    dispatcher_ = new SessionAlertDispatcher();
    session_ = new libtorrent::session();
}

Session::~Session()
{
    delete dispatcher_;
    delete session_;
}

void Session::load_state(LazyEntry^ e)
{
    session_->load_state(*e->ptr());
}

Entry^ Session::save_state(unsigned int flags)
{
    libtorrent::entry e;
    session_->save_state(e, flags);

    return gcnew Entry(e);
}

void Session::post_torrent_updates()
{
    session_->post_torrent_updates();
}

TorrentHandle^ Session::find_torrent(Sha1Hash^ hash)
{
    return gcnew TorrentHandle(session_->find_torrent(hash->ptr()));
}

cli::array<TorrentHandle^>^ Session::get_torrents()
{
    std::vector<libtorrent::torrent_handle> torrents = session_->get_torrents();
    cli::array<TorrentHandle^>^ result = gcnew cli::array<TorrentHandle^>(torrents.size());

    for (size_t i = 0; i < torrents.size(); i++)
    {
        result[i] = gcnew TorrentHandle(torrents[i]);
    }

    return result;
}

void Session::async_add_torrent(AddTorrentParams^ params)
{
    session_->async_add_torrent(*params->ptr());
}

TorrentHandle^ Session::add_torrent(AddTorrentParams^ params)
{
    libtorrent::error_code ec;
    libtorrent::torrent_handle h = session_->add_torrent(*params->ptr(), ec);

    if (ec)
    {
        throw gcnew System::Exception(interop::from_std_string(ec.message()));
    }

    return gcnew TorrentHandle(h);
}

void Session::pause()
{
    session_->pause();
}

void Session::resume()
{
    session_->resume();
}

bool Session::is_paused()
{
    return session_->is_paused();
}

SessionStatus^ Session::status()
{
    return gcnew SessionStatus(session_->status());
}


FeedHandle ^ Session::add_feed(FeedSettings ^ feed)
{
	libtorrent::feed_settings & s = *feed->ptr();

	if (s.url.empty())
	{
		throw gcnew System::Exception("Feed url empty");
	}
	libtorrent::feed_handle h = session_->add_feed(s);
	return gcnew FeedHandle(h);
}

void Session::remove_feed(FeedHandle ^ handle)
{
	session_->remove_feed(*handle->ptr());
}

cli::array<FeedHandle^>^ Session::get_feeds()
{
	std::vector<libtorrent::feed_handle> f;
	session_->get_feeds(f);
	size_t size = f.size();
	cli::array<FeedHandle ^ > ^ feeds = gcnew cli::array<FeedHandle ^ >(size);
	for (size_t i = 0; i < size; i++)
	{
		feeds[i] = gcnew FeedHandle(f[i]);
	}
	return feeds;
}

bool Session::is_dht_running()
{
    return session_->is_dht_running();
}

DhtSettings^ Session::get_dht_settings()
{
    return gcnew DhtSettings(session_->get_dht_settings());
}

void Session::start_dht()
{
    session_->start_dht();
}

void Session::stop_dht()
{
    session_->stop_dht();
}

void Session::set_dht_settings(DhtSettings^ settings)
{
    session_->set_dht_settings(settings->ptr());
}

void Session::add_dht_router(System::String^ host, int port)
{
    session_->add_dht_router(std::make_pair(interop::to_std_string(host), port));
}

void Session::add_dht_node(System::String^ host, int port)
{
    session_->add_dht_node(std::make_pair(interop::to_std_string(host), port));
}

Sha1Hash ^ Session::dht_put_item(Entry ^ entry)
{
	return gcnew Sha1Hash(session_->dht_put_item(*entry->ptr()));
}

void Session::dht_get_item(Sha1Hash ^ target)
{
	session_->dht_get_item(target->ptr());
}

void Session::dht_get_item(cli::array<char> ^ public_key, System::String ^ salt)
{
	if (public_key->Length != 32) return;
	boost::array<char, 32> k;
	for (size_t i = 0; i < 32; i++)
	{
		k[i] = public_key[i];
	}
	session_->dht_get_item(k, interop::to_std_string(salt));
}

void Session::load_country_db(System::String^ file)
{
    session_->load_country_db(interop::to_std_string(file).c_str());
}

void Session::load_asnum_db(System::String^ file)
{
    session_->load_asnum_db(interop::to_std_string(file).c_str());
}

void Session::set_key(int key)
{
    session_->set_key(key);
}

void Session::listen_on(int minPort, int maxPort)
{
    libtorrent::error_code ec;
    session_->listen_on(std::make_pair(minPort, maxPort), ec);

    if (ec)
    {
        throw gcnew System::Exception(interop::from_std_string(ec.message()));
    }
}

bool Session::is_listening()
{
    return session_->is_listening();
}

int Session::listen_port()
{
    return session_->listen_port();
}

int Session::ssl_listen_port()
{
    return session_->ssl_listen_port();
}

void Session::remove_torrent(TorrentHandle^ handle, int options)
{
    session_->remove_torrent(*handle->ptr(), options);
}

SessionSettings^ Session::settings()
{
    return gcnew SessionSettings(session_->settings());
}

void Session::set_settings(SessionSettings^ settings)
{
    session_->set_settings(settings->ptr());
}

void Session::set_alert_mask(AlertMask mask)
{
	session_->set_alert_mask(static_cast<uint32_t>(mask));
}

void Session::set_alert_dispatch(System::Action<Alert^>^ dispatch)
{
    dispatcher_->set_callback(dispatch);
    session_->set_alert_dispatch(std::bind(&SessionAlertDispatcher::invoke_callback, *dispatcher_, std::placeholders::_1));
}

void Session::clear_alert_dispatch()
{
    typedef boost::function<void(std::auto_ptr<libtorrent::alert>)> dispatch_func_t;
    session_->set_alert_dispatch(dispatch_func_t());
}

void Session::stop_lsd()
{
    session_->stop_lsd();
}

void Session::start_lsd()
{
    session_->start_lsd();
}

void Session::stop_upnp()
{
    session_->stop_upnp();
}

void Session::start_upnp()
{
    session_->start_upnp();
}

void Session::delete_port_mapping(int handle)
{
    session_->delete_port_mapping(handle);
}

void Session::stop_natpmp()
{
    session_->stop_natpmp();
}

void Session::start_natpmp()
{
    session_->start_natpmp();
}

void Session::post_dht_stats()
{
	session_->post_dht_stats();
}

void Session::post_session_stats()
{
	session_->post_session_stats();
}
