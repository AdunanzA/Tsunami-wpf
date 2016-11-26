using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tsunami.Models
{
    public class Part : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        private bool downloaded;
        private int priority;

        public int Id { get { return id; } set { if (id != value) { id = value; CallPropertyChanged("Id"); } } }
        public bool Downloaded { get { return downloaded; } set { if (downloaded != value) { downloaded = value; CallPropertyChanged("Downloaded"); CallPropertyChanged("Brush"); } } }
        public int Priority { get { return priority; } set { if (priority != value) { priority = value; CallPropertyChanged("Priority"); } } }
        public Brush Brush {
            get {
                if (downloaded)
                {
                    return Brushes.LightGreen;
                }
                else
                {
                    return Brushes.Gray;
                }
            }
        }

        public void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
