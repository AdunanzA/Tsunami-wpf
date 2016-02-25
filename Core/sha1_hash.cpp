#include "sha1_hash.h"



#include "interop.h"

using namespace Tsunami;

Sha1Hash::Sha1Hash(const libtorrent::sha1_hash& hash)
{
    hash_ = new libtorrent::sha1_hash(hash);
}

Sha1Hash::Sha1Hash(System::String^ val)
{
    std::string hex = interop::to_std_string(val);

    hash_ = new libtorrent::sha1_hash();
    libtorrent::from_hex(&hex[0], hex.size(), (char*)&hash_[0]);
}

Sha1Hash::~Sha1Hash()
{
    delete hash_;
}

libtorrent::sha1_hash& Sha1Hash::ptr()
{
    return *hash_;
}

System::String^ Sha1Hash::ToString()
{
    std::string hex = libtorrent::to_hex(hash_->to_string());
    return interop::from_std_string(hex);
}
