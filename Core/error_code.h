#pragma once

#include <libtorrent/error_code.hpp>

namespace Tsunami
{
	namespace Core
	{
		public ref class ErrorCode
		{
		internal:
			ErrorCode(const libtorrent::error_code& ec);

		public:
			~ErrorCode();

			System::String^ message();
			int value();

		private:
			libtorrent::error_code* error_;
		};
	}
}
