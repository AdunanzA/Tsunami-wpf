#pragma once

#include <libtorrent/sha1_hash.hpp>

namespace Tsunami
{
    public ref class Sha1Hash
    {
    internal:
        Sha1Hash(const libtorrent::sha1_hash& hash);
        libtorrent::sha1_hash& ptr();

    public:
        Sha1Hash(System::String^ val);
        ~Sha1Hash();

        System::String^ ToString() override;

    private:
        libtorrent::sha1_hash* hash_;
    };
}
