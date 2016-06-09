#include "peer_request.h"


using namespace Tsunami::Core;

#define PEER_INT_PROP(name) \
    int PeerRequest::name::get() \
    { \
        return request_->name; \
    }


PeerRequest::PeerRequest(libtorrent::peer_request& request)
{
	request_ = new libtorrent::peer_request(request);
}

PeerRequest::~PeerRequest()
{
	delete request_;
}

bool PeerRequest::operator==(PeerRequest ^ rhs)
{
	return piece == rhs->piece && start == rhs->start && length == rhs->length;
}

PEER_INT_PROP(piece);
PEER_INT_PROP(start);
PEER_INT_PROP(length);