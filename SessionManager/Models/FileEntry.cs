using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class FileEntry
    {
        public bool ExecutableAttribute { get; set; }
        public string Filehash { get; set; }
        public long FileBase { get; set; }
        public bool HiddenAttribute { get; set; }
        public DateTime Mtime { get; set; }
        public long Offset { get; set; }
        public bool PadFile { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public bool SymlinkAttribute { get; set; }
        public string SymlinkPath { get; set; }

        public FileEntry() { /* nothing to do. just for serializator */ }

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
    }
}
