#pragma once

#include <libtorrent/create_torrent.hpp>

namespace Tsunami
{
    ref class Entry;
    ref class FileStorage;
    ref class Sha1Hash;

    public ref class CreateTorrent
    {
    internal:
        libtorrent::create_torrent& ptr();

    public:
        CreateTorrent(FileStorage^ fs);
        CreateTorrent(FileStorage^ fs, int piece_size);
        CreateTorrent(FileStorage^ fs, int piece_size, int pad_file_limit);
        CreateTorrent(FileStorage^ fs, int piece_size, int pad_file_limit, int flags);
        CreateTorrent(FileStorage^ fs, int piece_size, int pad_file_limit, int flags, int alignment);
        ~CreateTorrent();

        static void add_files(FileStorage^ fs, System::String^ path);
        static void add_files(FileStorage^ fs, System::String^ path, unsigned int flags);
        static void set_piece_hashes(CreateTorrent^ ct, System::String^ path);

        Entry^ generate();
        FileStorage^ files();
        void set_comment(System::String^ str);
        void set_creator(System::String^ str);
        void set_hash(int index, Sha1Hash^ hash);
        void set_file_hash(int index, Sha1Hash^ hash);
        void add_url_seed(System::String^ url);
        void add_http_seed(System::String^ url);
        void add_node(System::String^ host, int port);
        void add_tracker(System::String^ url);
        void add_tracker(System::String^ url, int tier);
        void set_root_cert(System::String^ pem);
        bool priv();
        void set_priv(bool p);
        int num_pieces();
        int piece_length();
        int piece_size(int i);
        // TODO merkle tree

    private:
        libtorrent::create_torrent* create_;
    };
}
