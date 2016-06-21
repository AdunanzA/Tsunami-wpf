#pragma once

#pragma comment (lib, "advapi32.lib")

#ifdef _DEBUG
#pragma comment (lib, "opendhtd.lib")
#pragma comment (lib, "libgnutlsd.lib")
#else
#pragma comment (lib, "opendht.lib")
#pragma comment (lib, "libgnutls.lib")
#endif

#include <memory>
#include <string>

namespace Tsunami
{
	namespace Core
	{
		class Dht
			{
			public:
				Dht();
				virtual ~Dht();
				bool start();
				void bootstrap(const std::string & host, const std::string & service);
			private:
				struct tsunami_dht;
				std::unique_ptr<tsunami_dht> dht_;
			};
	}
}

