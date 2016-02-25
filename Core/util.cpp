#include "util.h"



#include "entry.h"
#include "interop.h"
#include "lazy_entry.h"

using namespace Tsunami;

cli::array<System::Byte>^ Util::bencode(Entry^ e)
{
    std::vector<char> buffer;
    libtorrent::bencode(std::back_inserter(buffer), *e->ptr());

    auto result = gcnew cli::array<System::Byte>(buffer.size());

    for (size_t i = 0; i < buffer.size(); i++)
    {
        result[i] = buffer[i];
    }

    return result;
}

LazyEntry^ Util::lazy_bdecode(cli::array<System::Byte>^ buffer)
{
    libtorrent::lazy_entry ret;
    libtorrent::error_code ec;

    pin_ptr<unsigned char> ptr = &buffer[0];
    const char *pbegin = (const char*)(const unsigned char*)ptr;
    libtorrent::lazy_bdecode(pbegin, pbegin + buffer->Length, ret, ec);

    if (ec)
    {
        throw gcnew System::Exception(interop::from_std_string(ec.message()));
    }

    return gcnew LazyEntry(ret);
}
