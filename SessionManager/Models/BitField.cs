using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class BitField
    {
        public bool AllSet { get; set; }
        public bool Empty { get; set; }
        public bool NoneSet { get; set; }
        public int NumWords { get; set; }
        public int Size { get; set; }

        public BitField() { /* nothing to do. just for serializator */ }

        public BitField(Core.BitField bf)
        {
            AllSet = bf.all_set;
            Empty = bf.empty;
            NoneSet = bf.none_set;
            NumWords = bf.num_words;
            Size = bf.size;
        }

        //public void ClearBit(int index)
        //{
        //    _bf.clear_bit(index);
        //}

        //public bool GetBit(int index)
        //{
        //    return _bf.get_bit(index);
        //}

        //public bool OpSubscript(int index)
        //{
        //    return _bf.op_Subscript(index);
        //}

        //public void SetBit(int index)
        //{
        //    _bf.set_bit(index);
        //}

    }
}
