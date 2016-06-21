#include "adunanza_dht.h"
#include "tsunami_dht.h"
#include "interop.h"

using namespace Tsunami::Core;

AdunanzaDht::AdunanzaDht()
{
	dht_ = new Dht();
}

AdunanzaDht::~AdunanzaDht()
{
	delete dht_;
}

bool AdunanzaDht::start()
{
	return dht_->start();
}

void AdunanzaDht::bootstrap(System::String ^ host, System::String ^ service)
{
	dht_->bootstrap(interop::to_std_string(host), interop::to_std_string(service));
}
