#include "session_alert_dispatcher.h"

#include "alert.h"



using namespace Tsunami::Core;

void SessionAlertDispatcher::invoke_callback(libtorrent::alert & al)
{
	gcroot<Alert^> a = Alert::Create2(&al);
	callback_->Invoke(a);
}

void SessionAlertDispatcher::set_callback(gcroot<System::Action<Alert^>^> callback)
{
	callback_ = callback;
}

void SessionDispatcher::invoke_callback()
{
	callback_->Invoke();
}

void SessionDispatcher::set_callback(gcroot<System::Action^> callback)
{
	callback_ = callback;
}


