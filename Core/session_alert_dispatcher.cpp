#include "session_alert_dispatcher.h"





using namespace Tsunami::Core;

void SessionAlertDispatcher::invoke_callback(std::auto_ptr<libtorrent::alert> al)
{
	gcroot<Alert^> a = Alert::create(al);
	System::Threading::Monitor::Enter(a);
	try
	{
		callback_->Invoke(a);
	}
	finally
	{
		System::Threading::Monitor::Exit(a);
	}
}

void SessionAlertDispatcher::set_callback(gcroot<System::Action<Alert^>^> callback)
{
	callback_ = callback;
}
