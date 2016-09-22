using System;

namespace Tsunami
{
    class StreamTorrent : IDisposable
    {
        private int last_have_piece = -1;
        private int starting_point = -1;
        private int num_have_pieces = -1;
        private int end_piece = -1;
        private Core.TorrentHandle _torrentHandle = null;
        private string _hash;
        private int piece_length = 0;
        private int num_pieces = 0;
        private string file_path;
        private bool invoke_done = false;
        private bool _onStreaming = false;

        public EventHandler<string> BufferReady;
        public bool OnStreaming
        {
            get
            {
                return _onStreaming;
            }
            set
            {
                _onStreaming = value;
            }
        }

        public StreamTorrent(string hash, int fileIndex,EventHandler<string> _callback)
        {
            if (_callback != null)
                BufferReady += _callback;
            _hash = hash;
            _torrentHandle = SessionManager.Instance.getTorrentHandle(hash);
            if (!_torrentHandle.has_metadata()) return;
            Core.TorrentInfo ti = _torrentHandle.torrent_file();
            var files = ti.files();
            if (fileIndex < 0 || fileIndex > files.num_files())
            {
                throw new ArgumentOutOfRangeException();
            }
            var fileEntry = files.at(fileIndex);
            file_path = Settings.User.PathDownload + "\\" + fileEntry.path;
            var peer_req = ti.map_file(fileIndex, 0, 1048576);
            starting_point = last_have_piece = peer_req.piece;
            piece_length = ti.piece_length();
            num_pieces = (int)Math.Ceiling((double)(fileEntry.size / piece_length));
            end_piece = Math.Min(last_have_piece + num_pieces, ti.num_pieces() - 1);

            //set first piece with higher priority
            _torrentHandle.piece_priority(last_have_piece, 7);
            _onStreaming = true;
        }

        public void ContinueStreaming(EventHandler<string> _callback)
        {
            if (_callback != null)
                BufferReady += _callback;
            _onStreaming = true;
            invoke_done = false;
             var all_pieces =  _torrentHandle.status().pieces;

            for (int i = starting_point; i < end_piece; i++)
            {
                
                if (!all_pieces.op_Subscript(i))
                    last_have_piece = starting_point + i; 
            }
            ContinueOne();
        }

        public void StopStreaming(EventHandler<string> _callback)
        {
            _onStreaming = false;
            BufferReady -= _callback;
        }

        public void ContinueOne()
        {
            num_have_pieces++;
            if (!invoke_done && (num_have_pieces * piece_length >= Settings.User.streamingBufferSize))
            {
                invoke_done = true;
                BufferReady?.Invoke(this,file_path);
            }
            _torrentHandle.piece_priority(last_have_piece, 7);
            last_have_piece++;
        }

        public void Dispose()
        {
        }
    }
}
