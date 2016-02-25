#pragma once

#include <memory>
#include <vcclr.h>

#include "alert.h"

namespace Tsunami
{
    ref class Alert;

    class SessionAlertDispatcher
    {
    public:
        void invoke_callback(std::auto_ptr<libtorrent::alert> alert);

        void set_callback(gcroot<System::Action<Alert^>^> callback);

    private:
        gcroot<System::Action<Alert^>^> callback_;
    };
}
