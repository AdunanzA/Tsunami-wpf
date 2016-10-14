#pragma once

#include <memory>

#include <libtorrent/alert.hpp>
#include <libtorrent/alert_types.hpp>

#include "enums.h"

namespace Tsunami
{
	namespace Core
	{
		public ref class Alert
		{
		internal:
			static Alert^ create(std::auto_ptr<libtorrent::alert> al);
			static Alert^ Create2(libtorrent::alert * al);
			Alert(libtorrent::alert* al);

		public:
			~Alert();

			System::DateTime timestamp();
			int type();
			System::String^ what();
			System::String^ message();
			int category();
			bool discardable();
			// TODO: clone?

		private:
			libtorrent::alert* alert_;
			static Alert^ SwitchType(libtorrent::alert *a);
		};
	}
}
