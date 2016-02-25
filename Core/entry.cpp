#include "entry.h"



using namespace Tsunami;

Entry::Entry(const libtorrent::entry& e)
{
    entry_ = new libtorrent::entry(e);
}

Entry::~Entry()
{
    delete entry_;
}

libtorrent::entry* Entry::ptr()
{
    return entry_;
}
