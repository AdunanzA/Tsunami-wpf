using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tsunami.Models
{
    public class BitField : INotifyPropertyChanged
    {
        public ObservableCollection<Part> Parts { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool AllSet { get; set; }
        public bool Empty { get; set; }
        public bool NoneSet { get; set; }
        public int NumWords { get; set; }
        public int Size { get; set; }        

        public BitField()
        {
            Parts = new ObservableCollection<Part>();
        }

        public BitField(Core.BitField bf)
        {
            Update(bf);
            Parts = new ObservableCollection<Part>();
            for (int i = 0; i < Size; i++)
            {
                Parts.Add(new Part() { Id = i, Downloaded = false, Priority = 4 });
            }
        }

        public void Update(Core.BitField bf)
        {
            AllSet = bf.all_set;
            Empty = bf.empty;
            NoneSet = bf.none_set;
            NumWords = bf.num_words;
            Size = bf.size;
        }

        public void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

}
