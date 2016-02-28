#pragma once

#include <memory>

#include <libtorrent/alert.hpp>
#include <libtorrent/alert_types.hpp>

namespace Tsunami
{
	namespace Core
	{
		public ref class Alert
		{
		internal:
			static Alert^ create(std::auto_ptr<libtorrent::alert> al);
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
		};
	}
}
