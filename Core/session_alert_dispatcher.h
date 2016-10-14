#pragma once

#include <memory>
#include <vcclr.h>

#include "alert.h"

namespace Tsunami
{
	namespace Core
	{
		ref class Alert;

		class SessionAlertDispatcher
		{
		public:
			void invoke_callback(libtorrent::alert & alert);

			void set_callback(gcroot<System::Action<Alert^>^> callback);

		private:
			gcroot<System::Action<Alert^>^> callback_;
		};

		class SessionDispatcher
		{
		public:
			SessionDispatcher(libtorrent::session & s)
			{
				session_ = &s;
			};
			void invoke_callback();
			void set_callback(gcroot<System::Action ^ > callback);
		private:
			libtorrent::session *session_;
		private:
			gcroot<System::Action ^> callback_;
		};
	}
}
