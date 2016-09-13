using System;

namespace Tsunami
{
    class StreamTorrent : IDisposable
    {
        private int last_have_piece = -1;
        private int num_have_pieces = 0;
        private int end_piece = -1;
        private Core.TorrentHandle _torrentHandle = null;
        private string _hash;
        private int piece_length;
        private int num_pieces = 0;

        //public EventHandler<string> BufferingCompleted;

        public StreamTorrent(string hash, int fileIndex)
        {
            _hash = hash;
            _torrentHandle = SessionManager.getTorrentHandle(hash);
            if (!_torrentHandle.has_metadata()) return;
            Core.TorrentInfo ti = _torrentHandle.torrent_file();
            var files = ti.files();
            if (fileIndex < 0 || fileIndex > files.num_files())
            {
                throw new ArgumentOutOfRangeException();
            }
            var fileEntry = files.at(fileIndex);
            var peer_req = ti.map_file(fileIndex, 0, 1048576);
            last_have_piece = peer_req.piece;
            piece_length = ti.piece_length();
            num_pieces = (int)Math.Ceiling((double)(fileEntry.size / piece_length));
            end_piece = Math.Min(last_have_piece + num_pieces, ti.num_pieces() - 1);
            for (int i = last_have_piece; i < end_piece; i++)
                _torrentHandle.piece_priority(i, 0);

            //set first piece with higher priority
            _torrentHandle.piece_priority(last_have_piece, 7);
        }

        public void ContinueStreaming()
        {
            while (_torrentHandle.have_piece(last_have_piece++));
            for (int i = last_have_piece; i < end_piece; i++)
                _torrentHandle.piece_priority(i, 0);
            ContinueOne();
        }

        public void StopStreaming()
        {
            for (int i = last_have_piece; i < end_piece; i++)
                _torrentHandle.piece_priority(i, 1);
        }

        public void ContinueOne()
        {
            num_have_pieces++;
            if (num_have_pieces * piece_length >= Settings.User.streamingBufferSize)
            {
                //BufferingCompleted?.Invoke();
            }
            _torrentHandle.piece_priority(last_have_piece++, 7);
        }

        public void Dispose()
        {
        }
    }
}
