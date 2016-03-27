#pragma once

#include <libtorrent/peer_info.hpp>
#include <libtorrent/peer_request.hpp>


namespace Tsunami
{
	namespace Core
	{
		public ref class PeerRequest
		{
		internal:

			PeerRequest(libtorrent::peer_request & request);

		public:
			~PeerRequest();

			property int piece { int get(); }
			property int start { int get(); }
			property int length { int get(); }

			bool operator== (PeerRequest^);

		private:
			libtorrent::peer_request* request_;
		};
	}
}
