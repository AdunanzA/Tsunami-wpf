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

void AdunanzaDht::put(System::String ^ key, cli::array<System::Byte> ^ data)
{
	std::vector<char> data_;
	data_.reserve(data->Length);
	for each (System::Byte i in data)
	{
		data_.push_back(i);
	}
	dht_->put(interop::to_std_string(key), data_);
}

cli::array<cli::array<System::Byte> ^> ^ AdunanzaDht::get(System::String ^ key)
{
	auto data = dht_->get(interop::to_std_string(key));
	cli::array<cli::array<System::Byte> ^> ^data_ = gcnew cli::array<cli::array<System::Byte> ^>(data.size());
	int j = 0;
	for each (auto v in data)
	{
		cli::array<System::Byte> ^ index = gcnew cli::array<System::Byte>(v.size());
		int k = 0;
		for each (auto i in v)
		{
			index[k++] = i;
		}
		data_[j++] = index;
	}
	return data_;
}
