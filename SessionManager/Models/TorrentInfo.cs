using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tsunami.Models
{
    public class TorrentInfo : IDisposable
    {
        private Core.TorrentInfo _ti;

        public TorrentInfo(Core.TorrentInfo ti)
        {
            _ti = ti;
        }

        public void AddTracker(string url, int tier)
        {
            _ti.add_tracker(url, tier);
        }

        public List<string> Collections()
        {
            List<string> sr = new List<string>();
            foreach (string item in _ti.collections())
            {
                sr.Add(item);
            }
            return sr;
        }

        public string Comment()
        {
            return _ti.comment();
        }

        public ValueType CreationDate()
        {
            return _ti.creation_date();
        }

        public string Creator()
        {
            return _ti.creator();
        }

        public FileStorage Files()
        {
            return new FileStorage(_ti.files());
        }

        public List<string> FileList()
        {
            List<string> sr = new List<string>();
            foreach (string item in _ti.file_list())
            {
                sr.Add(item);
            }
            return sr;
        }

        public string FilePath(int index)
        {
            return _ti.file_path(index);
        }

        public long FileSize(int index)
        {
            return _ti.file_size(index);
        }

        public string InfoHash()
        {
            return _ti.info_hash().ToString();
        }

        public bool IsI2p()
        {
            return _ti.is_i2p();
        }

        public bool IsMerkleTorrent()
        {
            return _ti.is_merkle_torrent();
        }

        public bool IsValid()
        {
            return _ti.is_valid();
        }

        public byte[] Metadata()
        {
            return _ti.metadata();
        }

        public int MetadataSize()
        {
            return _ti.metadata_size();
        }

        public string Name()
        {
           return _ti.name();
        }

        public int NumFiles()
        {
            return _ti.num_files();
        }

        public int NumPieces()
        {
            return _ti.num_pieces();
        }

        public FileStorage OrigFiles()
        {
            return new FileStorage(_ti.orig_files());
        }

        public int PieceLength()
        {
            return _ti.piece_length();
        }

        public int PieceSize(int index)
        {
            return _ti.piece_size(index);
        }

        public bool Priv()
        {
            return _ti.priv();
        }

        public void RemapFiles(FileStorage f)
        {
            _ti.remap_files(f.FileStorageCore());
        }

        public void RenameFile(int index, string new_filename)
        {
            _ti.rename_file(index, new_filename);
        }

        public List<string> SimilarTorrents()
        {
            List<string> sr = new List<string>();
            foreach (Core.Sha1Hash item in _ti.similar_torrents())
            {
                sr.Add(item.ToString());
            }
            return sr;
        }

        public string SslCert()
        {
            return _ti.ssl_cert();
        }

        public long TotalSize()
        {
            return _ti.total_size();
        }

        public List<AnnounceEntry> Trackers()
        {
            List<AnnounceEntry> sr = new List<AnnounceEntry>();
            foreach (Core.AnnounceEntry item in _ti.trackers())
            {
                sr.Add(new AnnounceEntry(item));
            }
            return sr;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ti.Dispose();
                }
                _ti = null;
                disposedValue = true;
            }
        }

        ~TorrentInfo()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
