#pragma once

namespace Tsunami
{
	namespace Core
	{
		class Dht;

		public ref class AdunanzaDht
		{
		public:
			AdunanzaDht();
			~AdunanzaDht();

			bool start();
			void bootstrap(System::String ^ host, System::String ^ service);
			void put(System::String ^ key, cli::array<System::Byte> ^ data);
			cli::array<cli::array<System::Byte> ^> ^ get(System::String ^ key);

		private:
			Dht * dht_;
		};
	}
}
