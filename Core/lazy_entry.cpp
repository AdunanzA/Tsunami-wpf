#include "lazy_entry.h"



using namespace Tsunami::Core;

LazyEntry::LazyEntry(libtorrent::lazy_entry& e)
{
    entry_ = new libtorrent::lazy_entry();
    entry_->swap(e);
}

LazyEntry::~LazyEntry()
{
    delete entry_;
}

libtorrent::lazy_entry* LazyEntry::ptr()
{
    return entry_;
}