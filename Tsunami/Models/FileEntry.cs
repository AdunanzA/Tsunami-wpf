using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class FileEntry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _executableAttribute { get; set; }
        private string _filehash { get; set; }
        private long _fileBase { get; set; }
        private bool _hiddenAttribute { get; set; }
        private DateTime _mtime { get; set; }
        private long _offset { get; set; }
        private bool _padFile { get; set; }
        private string _path { get; set; }
        private long _size { get; set; }
        private bool _symlinkAttribute { get; set; }
        private string _symlinkPath { get; set; }

        public string _fileName { get; set; }
        public bool _isValid { get; set; }
        public int _pieceSize { get; set; }


        public bool ExecutableAttribute { get { return _executableAttribute; } set { if (_executableAttribute != value) { _executableAttribute = value; CallPropertyChanged("ExecutableAttribute"); } } }
        public string Filehash  { get { return _filehash; } set { if (_filehash != value) { _filehash = value; CallPropertyChanged("Filehash"); } } }
        public long FileBase  { get { return _fileBase; } set { if (_fileBase != value) { _fileBase = value; CallPropertyChanged("FileBase"); } } }
        public bool HiddenAttribute  { get { return _hiddenAttribute; } set { if (_hiddenAttribute != value) { _hiddenAttribute = value; CallPropertyChanged("HiddenAttribute"); } } }
        public DateTime Mtime  { get { return _mtime; } set { if (_mtime != value) { _mtime = value; CallPropertyChanged("Mtime"); } } }
        public long Offset  { get { return _offset; } set { if (_offset != value) { _offset = value; CallPropertyChanged("Offset"); } } }
        public bool PadFile  { get { return _padFile; } set { if (_padFile != value) { _padFile = value; CallPropertyChanged("PadFile"); } } }
        public string Path  { get { return _path; } set { if (_path != value) { _path = value; CallPropertyChanged("Path"); } } }
        public long Size  { get { return _size; } set { if (_size != value) { _size = value; CallPropertyChanged("Size"); } } }
        public bool SymlinkAttribute  { get { return _symlinkAttribute; } set { if (_symlinkAttribute != value) { _symlinkAttribute = value; CallPropertyChanged("SymlinkAttribute"); } } }
        public string SymlinkPath  { get { return _symlinkPath; } set { if (_symlinkPath != value) { _symlinkPath = value; CallPropertyChanged("SymlinkPath"); } } }

        public string FileName  { get { return _fileName; } set { if (_fileName != value) { _fileName = value; CallPropertyChanged("FileName"); } } }
        public bool IsValid  { get { return _isValid; } set { if (_isValid != value) { _isValid = value; CallPropertyChanged("IsValid"); } } }
        public int PieceSize  { get { return _pieceSize; } set { if (_pieceSize != value) { _pieceSize = value; CallPropertyChanged("PieceSize"); } } }

        public FileEntry() {
        }

        public FileEntry(Core.FileEntry fe)
        {
            ExecutableAttribute = fe.executable_attribute;
            Filehash = fe.filehash.ToString();
            FileBase = fe.file_base;
            HiddenAttribute = fe.hidden_attribute;
            Mtime = fe.mtime;
            Offset = fe.offset;
            PadFile = fe.pad_file;
            Path = fe.path;
            Size = fe.size;
            SymlinkAttribute = fe.symlink_attribute;
            SymlinkPath = fe.symlink_path;
        }

        public void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
