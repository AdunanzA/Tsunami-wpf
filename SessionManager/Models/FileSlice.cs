using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class FileSlice
    {
        public int FileIndex { get; set; }
        public long Offset { get; set; }
        public long Size { get; set; }

        public FileSlice() { /* nothing to do. just for serializator */ }

        public FileSlice(Core.FileSlice fs)
        {
            FileIndex = fs.file_index;
            Offset = fs.offset;
            Size = fs.size;
        }

    }
}
