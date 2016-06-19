#pragma once

namespace Tsunami
{
	namespace Core
	{
		class Dht;

		ref class AdunanzaDht
		{
		public:
			AdunanzaDht();
			~AdunanzaDht();

			bool start();
			void bootstrap(System::String ^ host, System::String ^ service);

		private:
			Dht * dht_;
		};
	}
}
