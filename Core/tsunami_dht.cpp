#include "tsunami_dht.h"
#include <opendht.h>

using namespace Tsunami::Core;
using namespace dht;

struct Dht::tsunami_dht 
{
	DhtRunner node;
};

Dht::Dht()
	: dht_(new tsunami_dht())
{
	
}


Dht::~Dht()
{
	if (dht_->node.isRunning())
	{
		std::condition_variable cv;
		std::mutex m;
		std::atomic_bool done{ false };

		dht_->node.shutdown([&]()
		{
			std::lock_guard<std::mutex> lk(m);
			done = true;
			cv.notify_all();
		});

		// wait for shutdown
		std::unique_lock<std::mutex> lk(m);
		cv.wait(lk, [&]() { return done.load(); });

		dht_->node.join();
		gnutls_global_deinit();
	}
}

bool Dht::start()
{
	if(!dht_->node.isRunning())
	{
		if (int rc = gnutls_global_init())  
			throw std::runtime_error(std::string("Error initializing GnuTLS: ") + gnutls_strerror(rc));
		dht_->node.run(4222, {}, true, false);
		return true;
	}
	return false;
}

void Dht::bootstrap(const std::string & host, const std::string & service)
{
	dht_->node.bootstrap(host.c_str(), service.c_str());
}
