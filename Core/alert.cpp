#include "alert.h"

#include "alert_types.h"
#include "interop.h"

#define ALERT_CASE(alert_name) \
    case libtorrent::alert_name::alert_type: \
    { \
        libtorrent::alert_name* tmp = libtorrent::alert_cast<libtorrent::alert_name>(a); \
        return gcnew alert_name(tmp); \
    }

using namespace Tsunami::Core;

Alert::Alert(libtorrent::alert* al)
{
    alert_ = al;
}

Alert::~Alert()
{
    delete alert_;
}

Alert^ Alert::create(std::auto_ptr<libtorrent::alert> al)
{
    libtorrent::alert* a = al->clone().release();

    switch (a->type())
    {
        ALERT_CASE(torrent_added_alert);
        ALERT_CASE(torrent_removed_alert);
		ALERT_CASE(torrent_finished_alert);
		ALERT_CASE(state_update_alert);
    default:
        return gcnew Alert(a);
    }

    throw gcnew System::NotImplementedException();
}

System::DateTime Alert::timestamp()
{
	return System::DateTime::Now;
}

int Alert::type()
{
    return alert_->type();
}

System::String^ Alert::what()
{
    return interop::from_std_string(alert_->what());
}

System::String^ Alert::message()
{
    return interop::from_std_string(alert_->message());
}

int Alert::category()
{
    return alert_->category();
}

bool Alert::discardable()
{
    return alert_->discardable();
}
