#include "session_alert_dispatcher.h"





using namespace Tsunami::Core;

void SessionAlertDispatcher::invoke_callback(std::auto_ptr<libtorrent::alert> al)
{
    // TODO lock
    gcroot<Alert^> a = Alert::create(al);
    callback_->Invoke(a);
}

void SessionAlertDispatcher::set_callback(gcroot<System::Action<Alert^>^> callback)
{
    callback_ = callback;
}
