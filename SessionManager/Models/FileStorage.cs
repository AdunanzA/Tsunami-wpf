using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class FileStorage : IDisposable
    {
        Core.FileStorage _fs;

        public void AddFile(FileEntry entry)
        {
            _fs.add_file(entry.FileEntryCore());
        }

        public void AddFile(string p, long size)
        {
            _fs.add_file(p, size);
        }

        public FileEntry at(int index)
        {
            return new FileEntry(_fs.at(index));
        }

        public long FileBase(int index)
        {
            return _fs.file_base(index);
        }

        public int FileFlags(int index)
        {
            return _fs.file_flags(index);
        }

        public int FileIndexAtOffset(long offset)
        {
            return _fs.file_index_at_offset(offset);
        }

        public string FileName(int index)
        {
            return _fs.file_name(index);
        }

        public long FileOffset(int index)
        {
            return _fs.file_offset(index);
        }

        public string FilePath(int index, string save_path)
        {
            return _fs.file_path(index, save_path);
        }

        public long FileSize(int index)
        {
            return _fs.file_size(index);
        }

        public bool IsValid()
        {
            return _fs.is_valid();
        }

        public List<FileSlice> MapBlock(int piece, long offset, int size)
        {
            List<FileSlice> fs = new List<FileSlice>();
            Core.FileSlice[] cfs = _fs.map_block(piece, offset, size);

            foreach (Core.FileSlice item in cfs)
            {
                fs.Add(new FileSlice(item));
            }

            return fs;
        }

        public PeerRequest MapFile(int file, long offset, int size)
        {
            return new PeerRequest(_fs.map_file(file, offset, size));
        }

        public DateTime Mtime(int index)
        {
            return _fs.mtime(index);
        }

        public string Name()
        {
            return _fs.name();
        }

        public int NumFiles()
        {
            return _fs.num_files();
        }

        public int NumPieces()
        {
            return _fs.num_pieces();
        }

        public void Optimize(int pad_file_limit, int alignment)
        {
            _fs.optimize(pad_file_limit, alignment);
        }

        public bool PadFileAt(int index)
        {
            return _fs.pad_file_at(index);
        }

        public int PieceLength()
        {
            return _fs.piece_length();
        }

        public int PieceSize(int index)
        {
            return _fs.piece_size(index);
        }

        public void RenameFile(int index, string new_filename)
        {
            _fs.rename_file(index, new_filename);
        }

        public void Reserve(int num_files)
        {
            _fs.reserve(num_files);
        }

        public void SetFileBase(int index, long offset)
        {
            _fs.set_file_base(index, offset);
        }

        public void SetName(string name)
        {
            _fs.set_name(name);
        }

        public void SetNumPieces(int n)
        {
            _fs.set_num_pieces(n);
        }

        public void SetPieceLength(int l)
        {
            _fs.set_piece_length(l);
        }

        public string Symlink(int index)
        {
            return _fs.symlink(index);
        }

        public long TotalSize()
        {
            return _fs.total_size();
        }

        public FileStorage(Core.FileStorage fs)
        {
            _fs = fs;
        }

        public Core.FileStorage FileStorageCore()
        {
            return _fs;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_fs != null)
                    {
                        _fs.Dispose();
                    }
                }
                _fs = null;
                disposedValue = true;
            }
        }

        ~FileStorage()
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
