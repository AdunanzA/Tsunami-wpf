using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class TorrentInfo
    {
        public string[] Collections { get; set; }
        public string Comment { get; set; }
        public ValueType CreationDate { get; set; }
        public string Creator { get; set; }
        public string[] FileList { get; set; }
        public string InfoHash { get; set; }
        public bool IsI2p { get; set; }
        public bool IsMerkleTorrent { get; set; }
        public bool IsValid { get; set; }
        public byte[] Metadata { get; set; }
        public int MetadataSize { get; set; }
        public string Name { get; set; }
        public int NumFiles { get; set; }
        public int NumPieces { get; set; }
        public int PieceLength { get; set; }
        public bool Priv { get; set; }
        public string SslCert { get; set; }
        public long TotalSize { get; set; }

        //public FileStorage files();

        //public string FilePath(int index);
        //public long FileSize(int index);
        //public int PieceSize(int index);

        //public FileStorage orig_files();
        //public Sha1Hash[] similar_torrents();

        //public AnnounceEntry[] trackers();
    }
}
