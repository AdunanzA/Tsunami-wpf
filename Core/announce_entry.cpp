#include "announce_entry.h"


#include "interop.h"
#include "error_code.h"

using namespace Tsunami::Core;

AnnounceEntry::AnnounceEntry(libtorrent::announce_entry& entry)
{
    entry_ = new libtorrent::announce_entry(entry);
}

AnnounceEntry::AnnounceEntry(System::String^ url)
{
    entry_ = new libtorrent::announce_entry(interop::to_std_string(url));
}

AnnounceEntry::~AnnounceEntry()
{
    delete entry_;
}

libtorrent::announce_entry* AnnounceEntry::ptr()
{
    return entry_;
}

int AnnounceEntry::next_announce_in()
{
    return entry_->next_announce_in();
}

int AnnounceEntry::min_announce_in()
{
    return entry_->min_announce_in();
}

void AnnounceEntry::reset()
{
    entry_->reset();
}

bool AnnounceEntry::can_announce(System::DateTime now, bool is_seed)
{
    System::DateTime unix(1970, 1, 1);
    int totalSeconds = System::Convert::ToInt32((now - unix.ToLocalTime()).TotalSeconds);
	const std::chrono::seconds sc(totalSeconds);
    return entry_->can_announce(libtorrent::ptime(sc), is_seed); // TODO: investigate ptime
}

bool AnnounceEntry::is_working()
{
    return entry_->is_working();
}

void AnnounceEntry::trim()
{
    entry_->trim();
}

System::String^ AnnounceEntry::url::get()
{
    return interop::from_std_string(entry_->url);
}

System::String^ AnnounceEntry::trackerid::get()
{
    return interop::from_std_string(entry_->trackerid);
}

System::String^ AnnounceEntry::message::get()
{
    return interop::from_std_string(entry_->message);
}

ErrorCode ^ AnnounceEntry::last_error::get()
{
	return gcnew ErrorCode(entry_->last_error);
}

System::DateTime AnnounceEntry::next_announce::get()
{
	std::chrono::milliseconds ms = std::chrono::duration_cast<std::chrono::milliseconds>(entry_->next_announce.time_since_epoch());
	return System::DateTime(1970, 1, 1, 0, 0, 0, System::DateTimeKind::Utc).AddMilliseconds(static_cast<double>(ms.count()));
}

System::DateTime AnnounceEntry::min_announce::get()
{
	std::chrono::milliseconds ms = std::chrono::duration_cast<std::chrono::milliseconds>(entry_->min_announce.time_since_epoch());
	return System::DateTime(1970, 1, 1, 0, 0, 0, System::DateTimeKind::Utc).AddMilliseconds(static_cast<double>(ms.count()));
}

int AnnounceEntry::scrape_incomplete::get()
{
    return entry_->scrape_incomplete;
}

int AnnounceEntry::scrape_complete::get()
{
    return entry_->scrape_complete;
}

int AnnounceEntry::scrape_downloaded::get()
{
    return entry_->scrape_downloaded;
}

int AnnounceEntry::tier::get()
{
    return entry_->tier;
}

int AnnounceEntry::fail_limit::get()
{
    return entry_->fail_limit;
}

int AnnounceEntry::fails::get()
{
    return entry_->fails;
}

bool AnnounceEntry::updating::get()
{
    return entry_->updating;
}

int AnnounceEntry::source::get()
{
    return entry_->source;
}

bool AnnounceEntry::verified::get()
{
    return entry_->verified;
}

bool AnnounceEntry::start_sent::get()
{
    return entry_->start_sent;
}

bool AnnounceEntry::complete_sent::get()
{
    return entry_->complete_sent;
}

bool AnnounceEntry::send_stats::get()
{
    return entry_->send_stats;
}
