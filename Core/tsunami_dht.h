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
#include <vector>

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
				void put(const std::string & key, const std::vector<char> & data);
				std::vector<std::vector<uint8_t>> get(const std::string & key);
			private:
				struct tsunami_dht;
				std::unique_ptr<tsunami_dht> dht_;
			};
	}
}

