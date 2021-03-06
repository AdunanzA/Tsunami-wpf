#include "peer_info.h"



#include "interop.h"
#include "bitfield.h"
#include "sha1_hash.h"

using namespace Tsunami::Core;
using PeerId = Sha1Hash;

#define PEER_INT_PROP(name) \
    int PeerInfo::name::get() \
    { \
        return info_->name; \
    }

PeerInfo::PeerInfo(libtorrent::peer_info& info)
{
    info_ = new libtorrent::peer_info(info);
}

PeerInfo::~PeerInfo()
{
    delete info_;
}

unsigned int PeerInfo::flags::get()
{
    return info_->flags;
}

PEER_INT_PROP(source);
PEER_INT_PROP(up_speed);
PEER_INT_PROP(down_speed);
PEER_INT_PROP(payload_up_speed);
PEER_INT_PROP(payload_down_speed);

long long PeerInfo::total_download::get()
{
    return info_->total_download;
}

long long PeerInfo::total_upload::get()
{
    return info_->total_upload;
}

BitField ^ PeerInfo::pieces::get()
{
	return gcnew BitField(info_->pieces);
}

PeerId ^ PeerInfo::pid::get()
{
	return gcnew PeerId(info_->pid);
}

PEER_INT_PROP(upload_limit);
PEER_INT_PROP(download_limit);

System::TimeSpan PeerInfo::last_request::get()
{
    return System::TimeSpan(0, 0, (int)libtorrent::total_seconds(info_->last_request));
}

System::TimeSpan PeerInfo::last_active::get()
{
    return System::TimeSpan(0, 0, (int)libtorrent::total_seconds(info_->last_active));
}

System::TimeSpan PeerInfo::download_queue_time::get()
{
    return System::TimeSpan(0, 0, (int)libtorrent::total_seconds(info_->download_queue_time));
}

PEER_INT_PROP(queue_bytes);
PEER_INT_PROP(request_timeout);
PEER_INT_PROP(send_buffer_size);
PEER_INT_PROP(used_send_buffer);
PEER_INT_PROP(receive_buffer_size);
PEER_INT_PROP(used_receive_buffer);
PEER_INT_PROP(num_hashfails);

System::String^ PeerInfo::country::get()
{
    return interop::from_std_string(std::string(info_->country, 2));
}

System::String^ PeerInfo::inet_as_name::get()
{
	return interop::from_std_string(info_->client); //inet_as_name
}

PEER_INT_PROP(download_queue_length);
PEER_INT_PROP(timed_out_requests);
PEER_INT_PROP(busy_requests);
PEER_INT_PROP(requests_in_buffer);
PEER_INT_PROP(target_dl_queue_length);
PEER_INT_PROP(upload_queue_length);
PEER_INT_PROP(failcount);
PEER_INT_PROP(downloading_piece_index);
PEER_INT_PROP(downloading_block_index);
PEER_INT_PROP(downloading_progress);
PEER_INT_PROP(downloading_total);

System::String^ PeerInfo::client::get()
{
    return interop::from_std_string(info_->client);
}

PEER_INT_PROP(connection_type);
PEER_INT_PROP(remote_dl_rate);
PEER_INT_PROP(pending_disk_bytes);
PEER_INT_PROP(send_quota);
PEER_INT_PROP(receive_quota);
PEER_INT_PROP(rtt);
PEER_INT_PROP(num_pieces);
PEER_INT_PROP(download_rate_peak);
PEER_INT_PROP(upload_rate_peak);
PEER_INT_PROP(progress_ppm);
PEER_INT_PROP(estimated_reciprocation_rate);
