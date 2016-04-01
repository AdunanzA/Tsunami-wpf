#pragma once

#include <string>

namespace Tsunami
{
	namespace Core
	{
		namespace interop
		{
			System::String^ from_std_string(const std::string& value);
			std::string to_std_string(System::String^ value);
		}
	}
}


