#include "session_alert_dispatcher.h"

#include "alert.h"



using namespace Tsunami::Core;

void SessionDispatcher::invoke_callback()
{
	callback_->Invoke();
}

void SessionDispatcher::set_callback(gcroot<System::Action^> callback)
{
	callback_ = callback;
}


